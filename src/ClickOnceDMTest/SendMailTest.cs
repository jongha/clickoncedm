using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClickOnceDMTest
{
    [TestClass]
    public class SendMailTest
    {
        [TestMethod]
        public void SendMail()
        {
            SendMailProcess sendMailProcess = new SendMailProcess("smtp.test.com", 25);

            QueueProcess queueProcess = new QueueProcess();
            foreach (Queue queue in queueProcess.GetQueue())
            {
                sendMailProcess.Send(
                    new MailAddress("test@domain.com", "test"),
                    new MailAddress[] { new MailAddress("test@domain.com", "test") },
                    "subject|" + queue.Name + "|" + queue.Email,
                    "body|" + queue.Name + "|" + queue.Email
                    );
                
            }
        }
    }
}
