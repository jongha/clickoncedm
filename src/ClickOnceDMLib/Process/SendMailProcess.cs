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
                Port = this.port
            };

            smtp.Send(message);
        }
    }
}
