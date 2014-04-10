using ClickOnceDMLib.Structs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ClickOnceDMLib.Data
{
    public class Statistics
    {
        private string db;
        private const int DEFAULT_LIMIT = 50;

        public Statistics()
        {
            this.db = ConfigurationManager.AppSettings["StatisticsDatabase"];
        }

        public Statistics(string db)
        {
            this.db = db;
        }

        private SQLiteDatabase GetDatabase()
        {
            return new SQLiteDatabase(this.db);
        }

        public DataTable GetSearchStatistics(string subject)
        {
            return GetDatabase().GetDataTable(
                string.Format("select Subject, Success, Error, Timestamp from Statistics where Subject like '%{0}%'",
                    HttpUtility.HtmlEncode(subject))
                );
        }

        private DataTable GetStatistics(string subject)
        {
            return GetDatabase().GetDataTable(
                string.Format("select Subject, Success, Error, Timestamp from Statistics where Subject = '{0}'",
                    HttpUtility.HtmlEncode(subject)
                    )
                );
        }

        public bool SetStatistics(string subject, long success, long error)
        {
            subject = subject.Trim();

            DataTable dt = this.GetStatistics(subject);
            subject = HttpUtility.HtmlEncode(subject);

            if (dt.Rows.Count > 0)
            {
                long successSum = success, errorSum = error;

                foreach (DataRow dr in dt.Rows)
                {
                    successSum += Convert.ToInt64(dr["Success"]);
                    errorSum += Convert.ToInt64(dr["Error"]);
                }

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Subject", subject);
                data.Add("Success", successSum.ToString());
                data.Add("Error", errorSum.ToString());

                return GetDatabase().Update("Statistics", data, string.Format("Subject = '{0}'", subject));
            }
            else
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Subject", subject);
                data.Add("Success", success.ToString());
                data.Add("Error", error.ToString());

                return GetDatabase().Insert("Statistics", data);
            }
        }

        public bool DeleteStatisticsy(string subject)
        {
            return GetDatabase().ExecuteNonQuery(
                string.Format("delete from Statistics where Subject = '{0}'",
                    HttpUtility.HtmlEncode(subject))
                ) > 0;
        }

        public bool ClearStatistics(int days)
        {
            return GetDatabase().ExecuteNonQuery(
                string.Format("delete from Statistics where Timestamp < '{0}'", DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd HH:mm:ss"))
                ) > 0;
        }
    }
}
