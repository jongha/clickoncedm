using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ClickOnceDMService
{
    public partial class Service : ServiceBase
    {
        private static bool processing = false;
        private System.Timers.Timer timer;
        private Object lockObj = new Object();
        
        private class SMTPServer
        {
            public string Host = "localhost";
            public int Port = 25;
            public long Weight = 0;

            public void SetWeight(int weight)
            {
                this.Weight += (long)weight;
                this.Weight = Math.Min(this.Weight, 549755813888);
            }
        }

        private List<SMTPServer> smtpServer = null;

        public Service()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(5000);
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
        }

        private void InitSMTPServer()
        {
            LogProcess.Info("Initialize SMTP Server Information");

            this.smtpServer = new List<SMTPServer>();

            foreach (string server in ConfigurationManager.AppSettings["SMTPServer"].Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] trimedServer = server.Trim().Split(new char[] { ':' });
                smtpServer.Add(new SMTPServer()
                {
                    Host = trimedServer[0].Trim(),
                    Port = Convert.ToInt32(trimedServer[1].Trim()),
                    Weight = 0
                });
            }
        }

        protected override void OnStart(string[] args)
        {
            LogProcess.Info("Start");

            InitSMTPServer();

            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            LogProcess.Info("Stop");

            timer.Enabled = false;
            timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (processing || this.smtpServer == null) { return; }
            lock (lockObj)
            {
                processing = true;
            }

            TicketProcess ticketProcess = new TicketProcess();
            QueueProcess queueProcess = new QueueProcess();

            int blockSleep = Convert.ToInt32(ConfigurationManager.AppSettings["BlockSleep"]);

            queueProcess.BuildQueueFromTicket(
                ticketProcess, 
                Convert.ToInt32(ConfigurationManager.AppSettings["BlockCount"])
                );

            
            ClickOnceDMLib.Structs.Queue queue = queueProcess.GetQueue();

            if (queue != null)
            {
                InitSMTPServer();

                LogProcess.Info("In-Process Start");

                foreach (Recipient recipient in queue.RecipientData)
                {
                    long baseTick = DateTime.Now.Ticks;

                    var smtp = from s1 in this.smtpServer
                               orderby s1.Weight ascending
                               select s1;

                    SMTPServer serverInfo = smtp.First();

                    SendMailProcess sendMailProcess = new SendMailProcess(serverInfo.Host, serverInfo.Port);

                    MailAddress mailAddress = null;

                    try
                    {
                        mailAddress = new MailAddress(recipient.Address.Trim(), recipient.Name);
                    }
                    catch (Exception ex)
                    {
                        LogProcess.Error(ex.Message);
                        continue;
                    }

                    if (mailAddress != null)
                    {
                        sendMailProcess.Send(
                            new MailAddress(queue.TicketData.SenderAddress, queue.TicketData.SenderName),
                            new MailAddress[] { mailAddress },
                            queue.TicketData.Subject,
                            queue.TicketData.Body
                            );

                        serverInfo.SetWeight(TimeSpan.FromTicks(DateTime.Now.Ticks - baseTick).Milliseconds);
                    }
                }

                LogProcess.Info("In-Process End");

                Thread.Sleep(blockSleep);
            }

            processing = false;
        }
    }
}
