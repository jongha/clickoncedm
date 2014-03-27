using ClickOnceDMLib.Structs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

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

        public DataTable GetStatistics(DateTime start, DateTime end)
        {
            return GetDatabase().GetDataTable(
                string.Format("select Success, Error from Statistics where Timestamp between '{0}' and '{1}' order by Timestamp desc", 
                start.ToString("yyyy-MM-dd HH:mm:ss"), 
                end.ToString("yyyy-MM-dd HH:mm:ss"))
                );
        }

        public bool SetStatistics(long success, long error)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Success", success.ToString());
            data.Add("Error", error.ToString());

            return GetDatabase().Insert("Statistics", data);
        }

        public bool ClearStatistics(int days)
        {
            return GetDatabase().ExecuteNonQuery(
                string.Format("delete from Statistics where Timestamp < '{0}'", DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd HH:mm:ss"))
                ) > 0;
        }
    }
}
