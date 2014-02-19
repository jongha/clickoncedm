using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace ClickOnceDMLib.Process
{
    public class SendMailProcess
    {
        private string host = "localhost";
        private int port = 25;

        public SendMailProcess()
        {
        }

        public SendMailProcess(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Send(MailAddress sender, MailAddress[] recipients, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage()
                {
                    Priority = MailPriority.Normal,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                    From = sender,
                    Subject = subject.Trim(),
                    Body = body,
                    IsBodyHtml = true,
                };

                foreach (MailAddress recipient in recipients)
                {
                    message.To.Add(recipient);
                }

                SmtpClient smtp = new SmtpClient()
                {
                    Host = this.host,
                    Port = this.port,

                };

                string logMessage = string.Join(";", (
                    from recipient in recipients
                    select recipient.Address + ", " + recipient.DisplayName + string.Format(", ({0}:{1})", this.host, this.port)
                ).ToArray());

                try
                {
                    smtp.Send(message);

                    // write log
                    LogProcess.Info(logMessage);
                }
                catch (Exception e)
                {
                    LogProcess.Error(logMessage + ", " + e.Message);
                }
            }
            catch (Exception e)
            {
                LogProcess.Error(e.Message);
            }
        }
    }
}
