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
        public static void WriteErrorLog(Exception e)
        {
            WriteErrorLog(e.Message);
        }

        public static void WriteLog(Exception e)
        {
            WriteLog(e.Message);
        }

        public static void WriteLog(string message)
        {
            try
            {
                string file = PathInfo.CombinePath(PathInfo.Log, DateTime.Now.ToString("yyyyMMdd") + ".log");
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), message));
                }
            }
            catch { }
        }

        public static void WriteErrorLog(string message)
        {
            try
            {
                string file = PathInfo.CombinePath(PathInfo.Log, DateTime.Now.ToString("yyyyMMdd") + ".err");
                using (StreamWriter writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString(), message));
                }
            }
            catch { }
        }
    }
}
