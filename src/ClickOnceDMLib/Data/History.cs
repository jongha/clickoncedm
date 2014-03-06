using ClickOnceDMLib.Structs;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Data
{
    public class History
    {
        private const int DEFAULT_LIMIT = 50;

        private Database GetDatabase()
        {
            return new SqlDatabase(ConfigurationManager.ConnectionStrings["__LOCALSTORAGE__"].ToString());
        }

        public Ticket GetHistoryToTicket(int id)
        {
            Ticket ticket = null;

            DataSet ds = GetDatabase().ExecuteDataSet(
                CommandType.Text,
                "select Id, SenderName, SenderAddress, Subject, Body, Timestamp from History where Id = " + id.ToString()
                );

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ticket = new Ticket();
                    ticket.SenderAddress = dr["SenderAddress"].ToString();
                    ticket.SenderName = dr["SenderName"].ToString();
                    ticket.Subject = dr["Subject"].ToString();
                    ticket.Body = dr["Body"].ToString();

                    break;
                }
            }

            return ticket;
        }

        public DataSet GetHistory()
        {
            return this.GetHistory(DEFAULT_LIMIT);
        }

        public DataSet GetHistory(int limit)
        {
            if (limit <= 0)
            {
                limit = DEFAULT_LIMIT;
            }

            return GetDatabase().ExecuteDataSet(
                CommandType.Text,
                "select top " + limit.ToString() + " Id, SenderName, SenderAddress, Subject, Body, Timestamp from History order by Id desc"
                );
        }

        public bool SetHistory(Ticket ticket)
        {
            return GetDatabase().ExecuteNonQuery(
                CommandType.Text,
                string.Format("insert into History(SenderName, SenderAddress, Subject, Body) values('{0}', '{1}', '{2}', '{3}');", ticket.SenderName, ticket.SenderAddress, ticket.Subject, ticket.Body)
                ) > 0;
        }

        public bool DeleteHistory(int days)
        {
            return GetDatabase().ExecuteNonQuery(
                CommandType.Text,
                string.Format("delete from History where timestamp < (getutcdate() - {0})", days)
                ) > 0;
        }
    }
}
