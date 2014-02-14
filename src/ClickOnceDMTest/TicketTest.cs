using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ClickOnceDMLib;
using System.Runtime.Serialization.Json;
using System.Linq;
using ClickOnceDMLib.Path;
using ClickOnceDMLib.Structs;
using System.Collections.Generic;

namespace ClickOnceDMTest
{
    [TestClass]
    public class TicketTest
    {
        private const string TESTSTRING = "TEST";

        [TestMethod]
        public void MakeTicket()
        {
            int count = Directory.GetFiles(PathInfo.Ticket).Length;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() + ".ticket";
            
            TicketInfo ticketInfo = new TicketInfo();
            ticketInfo.SaveTicket(fileName, new SourceInfo
                {
                    Source = TESTSTRING,
                    Value = TESTSTRING,
                    ConnectionString = TESTSTRING
                });


            Assert.IsTrue(Directory.GetFiles(PathInfo.Ticket).Length == ++count);

            ticketInfo = new TicketInfo();
            List<SourceInfo> sourceInfo = ticketInfo.GetSources();
            
            Assert.IsTrue(sourceInfo.Count > 0);
        }

        [TestMethod]
        public void GetTickets()
        {
            TicketInfo ticketInfo = new TicketInfo();
            List<SourceInfo> sourceInfo = ticketInfo.GetSources();

            SourceInfo source = sourceInfo[0];
            string connect = source.ConnectionString;

            Assert.IsTrue(source.ConnectionString == TESTSTRING);
        }
    }
}