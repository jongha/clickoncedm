using ClickOnceDMLib.Process;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace ClickOnceDMService
{
    public class QueueService : IProcess
    {
        private static bool processing = false;
        private Object lockObj = new Object();

        public void Process()
        {
            if (processing) { return; }
            lock (lockObj)
            {
                processing = true;
            }

            TicketProcess ticketProcess = new TicketProcess();
            QueueProcess queueProcess = new QueueProcess();

            queueProcess.BuildQueueFromTicket(
                ticketProcess,
                Convert.ToInt32(ConfigurationManager.AppSettings["BlockCount"])
                );

            processing = false;
        }
    }
}
