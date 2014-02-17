using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ClickOnceDMLib;
using System.Runtime.Serialization.Json;
using System.Linq;
using ClickOnceDMLib.Path;
using ClickOnceDMLib.Structs;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using ClickOnceDMLib.Process;

namespace ClickOnceDMTest
{
    [TestClass]
    public class TicketTest
    {
        private const string TESTSTRING = "TEST";

        [TestMethod]
        public void MakeTicket()
        {
            TicketProcess ticketProcess = new TicketProcess();

            int count = ticketProcess.GetTickets().Count;

            string fileName = string.Empty;

            ticketProcess.SaveTicket(new Source
                {
                    Provider = "System.Data.SqlClient",
                    Value = "select top 1000 userName as name, userEmail as email from Member",
                    ConnectionString = "Data Source=tiger02;Initial Catalog=ALToolsMember;Integrated Security=False;User Id=svcinfra;Password=dkdlvhsy10;MultipleActiveResultSets=True"
                }, out fileName);

            Assert.IsTrue(Directory.GetFiles(PathInfo.Ticket).Length == ++count);

            ticketProcess = new TicketProcess();
            List<Source> sourceInfo = ticketProcess.GetSources();
            
            Assert.IsTrue(sourceInfo.Count > 0);
        }

        [TestMethod]
        public void GetTickets()
        {
            TicketProcess ticketProcess = new TicketProcess();
            List<Source> sourceInfo = ticketProcess.GetSources();

            Source source = sourceInfo[0];

            Assert.IsTrue(!string.IsNullOrEmpty(source.Provider));
            Assert.IsTrue(!string.IsNullOrEmpty(source.Value));
            Assert.IsTrue(!string.IsNullOrEmpty(source.ConnectionString));
        }

        [TestMethod]
        public void GetSourceFromTicket()
        {
            TicketProcess ticketProcess = new TicketProcess();
            List<Source> sourceInfo = ticketProcess.GetSources();

            Source source = sourceInfo[0];

            string value = source.Value;
            string connectionString = source.ConnectionString;

            if (source.Provider == "System.Data.SqlClient")
            {
                Database db = new SqlDatabase(source.ConnectionString);
                DataSet ds = db.ExecuteDataSet(CommandType.Text, source.Value);

                Assert.IsTrue(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 0);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        [TestMethod]
        private void EmptyTicket()
        {
            TicketProcess ticketProcess = new TicketProcess();
            ticketProcess.EmptyTicket();

            Assert.IsTrue(ticketProcess.GetTickets().Count == 0);
        }

        [TestMethod]
        public void RemoveTicket()
        {
            EmptyTicket();

            MakeTicket();

            TicketProcess ticketProcess = new TicketProcess();
            List<Ticket> tickets = ticketProcess.GetTickets();

            ticketProcess.RemoveTicket(tickets[0]);

            Assert.IsTrue(tickets.Count == (ticketProcess.GetTickets().Count + 1));
        }
    }
}