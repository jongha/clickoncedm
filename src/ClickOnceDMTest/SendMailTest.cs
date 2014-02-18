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
            QueueTest queueTest = new QueueTest();
            queueTest.BuildQueue();

            SendMailProcess sendMailProcess = new SendMailProcess("smtp.hostname.com", 25);

            QueueProcess queueProcess = new QueueProcess();
            Queue queue = queueProcess.GetQueue();

            foreach(Recipient recipient in queue.RecipientData)
            {
                sendMailProcess.Send(
                    new MailAddress(queue.TicketData.SenderAddress, queue.TicketData.SenderName),
                    new MailAddress[] { new MailAddress(recipient.Address, recipient.Name) },
                    queue.TicketData.Subject,
                    queue.TicketData.Body
                    );
            }
        }
    }
}
