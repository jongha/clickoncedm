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
        
        private class SMTPServer
        {
            public string Host = "localhost";
            public int Port = 25;
            public long Weight = 0;

            public void AddWeight(long weight)
            {
                this.Weight += weight;
            }
        }

        private List<SMTPServer> smtpServer = null;

        public Service()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(10000);
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
        }

        private void InitSMTPServer()
        {
            this.smtpServer = new List<SMTPServer>();

            foreach (string server in ConfigurationManager.AppSettings["SMTPServer"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
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
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (processing || this.smtpServer == null) { return; }
            lock (typeof(Service))
            {
                processing = true;
            }

            TicketProcess ticketProcess = new TicketProcess();
            QueueProcess queueProcess = new QueueProcess();

            queueProcess.BuildQueueFromTicket(ticketProcess);

            
            ClickOnceDMLib.Structs.Queue queue = queueProcess.GetQueue();

            if (queue != null)
            {
                InitSMTPServer();

                foreach (Recipient recipient in queue.RecipientData)
                {
                    long baseTick = DateTime.Now.Ticks;

                    var smtp = from s1 in this.smtpServer
                            orderby s1.Weight ascending
                            select s1;

                    SMTPServer serverInfo = smtp.First();

                    try
                    {
                        SendMailProcess sendMailProcess = new SendMailProcess(serverInfo.Host, serverInfo.Port);

                        sendMailProcess.Send(
                            new MailAddress(queue.TicketData.SenderAddress, queue.TicketData.SenderName),
                            new MailAddress[] { new MailAddress(recipient.Address, recipient.Name) },
                            queue.TicketData.Subject,
                            queue.TicketData.Body
                            );
                    }
                    catch (Exception ex)
                    {
                        LogProcess.WriteErrorLog(ex);
                    }

                    serverInfo.AddWeight(TimeSpan.FromTicks(DateTime.Now.Ticks - baseTick).Milliseconds);
                }
            }

            processing = false;
        }
    }
}
