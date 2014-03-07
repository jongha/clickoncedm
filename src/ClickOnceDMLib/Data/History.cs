using ClickOnceDMLib.Structs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ClickOnceDMLib.Data
{
    public class History
    {
        private string db;
        private const int DEFAULT_LIMIT = 50;

        public History()
        {
            this.db = ConfigurationManager.AppSettings["Database"];
        }

        public History(string db)
        {
            this.db = db;
        }

        private SQLiteDatabase GetDatabase()
        {
            return new SQLiteDatabase(this.db);
        }

        public Ticket GetHistoryToTicket(int id)
        {
            Ticket ticket = null;

            DataTable dt = GetDatabase().GetDataTable(
                "select Id, SenderName, SenderAddress, Subject, Body, Timestamp from History where Id = " + id.ToString()
                );

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ticket = new Ticket();
                    ticket.SenderAddress = HttpContext.Current.Server.HtmlDecode(dr["SenderAddress"].ToString());
                    ticket.SenderName = HttpContext.Current.Server.HtmlDecode(dr["SenderName"].ToString());
                    ticket.Subject = HttpContext.Current.Server.HtmlDecode(dr["Subject"].ToString());
                    ticket.Body = HttpContext.Current.Server.HtmlDecode(dr["Body"].ToString());

                    break;
                }
            }

            return ticket;
        }

        public DataTable GetHistory()
        {
            return this.GetHistory(DEFAULT_LIMIT);
        }

        public DataTable GetHistory(int limit)
        {
            if (limit <= 0)
            {
                limit = DEFAULT_LIMIT;
            }

            return GetDatabase().GetDataTable(
                "select Id, SenderName, SenderAddress, Subject, Body, Timestamp from History order by Id desc limit " + limit.ToString()
                );
        }

        public bool SetHistory(Ticket ticket)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("SenderName", HttpContext.Current.Server.HtmlEncode(ticket.SenderName));
            data.Add("SenderAddress", HttpContext.Current.Server.HtmlEncode(ticket.SenderAddress));
            data.Add("Subject", HttpContext.Current.Server.HtmlEncode(ticket.Subject));
            data.Add("Body", HttpContext.Current.Server.HtmlEncode(ticket.Body));
            data.Add("Timestamp", DateTime.Now.Ticks.ToString());

            return GetDatabase().Insert("History", data);
        }

        public bool DeleteHistory(int id)
        {
            return GetDatabase().ExecuteNonQuery(
                string.Format("delete from History where Id = {0}", id)
                ) > 0;
        }

        public bool ClearHistory(int days)
        {
            return GetDatabase().ExecuteNonQuery(
                string.Format("delete from History where Timestamp < {0}", DateTime.Now.AddDays(-days).Ticks)
                ) > 0;
        }
    }
}
