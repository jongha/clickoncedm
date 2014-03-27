using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Process
{
    public class LogCounterProcess
    {
        private static long success = 0;
        private static long error = 0;

        public static void SaveCounter()
        {
            success = 0;
            error = 0;
        }

        public static void Error(Exception e, string message)
        {
            ++error;

            LogProcess.Error(e, message);
        }

        public static void Error(Exception e)
        {
            ++error;

            LogProcess.Error(e, string.Empty);
        }

        public static void Success(string message)
        {
            ++success;

            LogProcess.WriteLog(LogProcess.LOGTYPE.SUCCESS, message);
        }
    }
}
