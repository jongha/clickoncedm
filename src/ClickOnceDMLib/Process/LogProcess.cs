using ClickOnceDMLib.Path;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Process
{
    public class LogProcess
    {
        private enum LOGTYPE { INFO, ERROR };

        public static void Error(string message)
        {
            WriteLog(LOGTYPE.ERROR, message);
        }

        public static void Info(string message)
        {
            WriteLog(LOGTYPE.INFO, message);
        }

        private static void WriteLog(LOGTYPE logType, string message)
        {
            string file = PathInfo.CombinePath(PathInfo.Log, DateTime.Now.ToString("yyyyMMddHH"));

            if (logType == LOGTYPE.INFO)
            {
                file += ".log";
            }
            else if(logType == LOGTYPE.ERROR)
            {
                file += ".error";
            }

            using (StreamWriter writer = new StreamWriter(file, true))
            {
                writer.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("u"), message));
            }
        }
    }
}
