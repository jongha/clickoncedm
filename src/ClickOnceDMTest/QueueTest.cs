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
        public void BuildQueue()
        {
            TicketTest ticketTest = new TicketTest();
            ticketTest.MakeTicket();

            //TicketInfo.GetTickets();
            TicketProcess ticketProcess = new TicketProcess();

            QueueProcess queueProcess = new QueueProcess();
            queueProcess.BuildQueueFromTicket(ticketProcess, 100);
        }

        [TestMethod]
        public void GetQueue()
        {
            QueueProcess queue = new QueueProcess();
            Queue queues = queue.GetQueue();

            Assert.IsTrue(queues != null);
        }
    }
}
