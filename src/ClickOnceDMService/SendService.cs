using ClickOnceDMLib.Interfaces;
using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using ClickOnceDMLib.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace ClickOnceDMService
{
    public class SendService : IProcess
    {
        private static bool processing = false;
        private Object lockObj = new Object();
        private List<SMTPServer> smtpServer = null;

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

        public SendService()
        {
            InitSMTPServer();
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

        public void Process()
        {
            if (processing || this.smtpServer == null) { return; }
            lock (lockObj)
            {
                processing = true;
            }

            QueueProcess queueProcess = new QueueProcess();

            int blockSleep = Convert.ToInt32(ConfigurationManager.AppSettings["BlockSleep"]);

            ClickOnceDMLib.Structs.Queue queue = queueProcess.GetQueue();

            if (queue != null)
            {
                InitSMTPServer();

                LogProcess.Info("In-Process Start");

                ILog log = new LogCounter(); // log interface

                foreach (Recipient recipient in queue.RecipientData)
                {
                    long baseTick = DateTime.Now.Ticks;

                    var smtp = from s1 in this.smtpServer
                               orderby s1.Weight ascending
                               select s1;

                    SMTPServer serverInfo = smtp.First();

                    SendMailProcess sendMailProcess = new SendMailProcess(log);

                    sendMailProcess.SetHostAndPort(serverInfo.Host, serverInfo.Port);

                    MailAddress mailAddress = null;

                    try
                    {
                        mailAddress = new MailAddress(recipient.Address.Trim(), recipient.Name);
                    }
                    catch (Exception ex)
                    {
                        LogProcess.Error(ex);
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

                log.Flush(); // write log

                LogProcess.Info("In-Process End");

                Thread.Sleep(blockSleep);
            }

            processing = false;
        }
    }
}
