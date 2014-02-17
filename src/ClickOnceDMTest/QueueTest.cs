using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickOnceDMTest
{
    [TestClass]
    public class QueueTest
    {
        [TestMethod]
        public void BuildTicket()
        {
            TicketTest ticketTest = new TicketTest();
            ticketTest.MakeTicket();

            //TicketInfo.GetTickets();
            TicketProcess ticketProcess = new TicketProcess();

            QueueProcess queueProcess = new QueueProcess();
            queueProcess.BuildQueueFromTicket(ticketProcess);
        }

        [TestMethod]
        public void GetQueue()
        {
            QueueProcess queue = new QueueProcess();
            List<Queue> queues = queue.GetQueue();

            Assert.IsTrue(queues.Count >= 0);
        }
    }
}
