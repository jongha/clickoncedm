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
        public enum LOGTYPE { INFO, SUCCESS, ERROR };

        public static void Error(Exception e, string message)
        {
            WriteLog(LOGTYPE.ERROR, 
                "\r\nMessage: " + e.Message +
                "\r\nStackTrace: " + e.StackTrace + 
                (!string.IsNullOrEmpty(message) ? "\r\nCustom Message: " + message : string.Empty)
                );
        }

        public static void Error(Exception e)
        {
            Error(e, string.Empty);
        }

        public static void Info(string message)
        {
            WriteLog(LOGTYPE.INFO, message);
        }

        public static void WriteLog(LOGTYPE logType, string message)
        {
            string file = string.Empty;
            switch (logType)
            {
                default:
                case LOGTYPE.INFO:
                case LOGTYPE.SUCCESS:
                    file = PathInfo.CombinePath(PathInfo.Log, DateTime.Now.ToString("yyyyMMddHH")) + "-access.log";
                    break;

                case LOGTYPE.ERROR:
                    file = PathInfo.CombinePath(PathInfo.Log, DateTime.Now.ToString("yyyyMMddHH")) + "-error.log";
                    break;
            }

            if (!string.IsNullOrEmpty(file))
            {
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("u"), message));
                }
            }
        }
    }
}
