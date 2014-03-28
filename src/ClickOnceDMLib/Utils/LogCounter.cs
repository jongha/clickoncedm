using ClickOnceDMLib.Data;
using ClickOnceDMLib.Interfaces;
using ClickOnceDMLib.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Utils
{
    public class LogCounter : ILog
    {
        private class Counter
        {
            public long Success;
            public long Error;

            public Counter()
            {
                this.Success = 0;
                this.Error = 0;
            }
        }

        private Dictionary<string, Counter> dataCounter;

        public LogCounter()
        {
            dataCounter = new Dictionary<string, Counter>(7);
        }

        public void Error(string key, Exception e, string message)
        {
            if (!dataCounter.ContainsKey(key))
            {
                dataCounter.Add(key, new Counter()
                {
                    Success = 0,
                    Error = 0
                });
            }

            ++((Counter)dataCounter[key]).Error;

            LogProcess.Error(e, message);
        }

        public void Error(string key, Exception e)
        {
            if (!dataCounter.ContainsKey(key))
            {
                dataCounter.Add(key, new Counter()
                {
                    Success = 0,
                    Error = 0
                });
            }

            ++((Counter)dataCounter[key]).Error;

            LogProcess.Error(e, string.Empty);
        }

        public void Success(string key, string message)
        {
            if (!dataCounter.ContainsKey(key))
            {
                dataCounter.Add(key, new Counter()
                {
                    Success = 0,
                    Error = 0
                });
            }

            ++((Counter)dataCounter[key]).Success;

            LogProcess.WriteLog(LogProcess.LOGTYPE.SUCCESS, message);
        }

        public void Flush()
        {
            Statistics statistics = new Statistics();

            foreach (string key in dataCounter.Keys)
            {
                Counter counter = (Counter)dataCounter[key];
                statistics.SetStatistics(key, counter.Success, counter.Error);
            }

            dataCounter = new Dictionary<string, Counter>(7);
        }
    }
}