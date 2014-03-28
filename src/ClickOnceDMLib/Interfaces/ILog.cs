using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Interfaces
{
    public interface ILog
    {
        void Error(string key, Exception e, string message);
        void Error(string key, Exception e);
        void Success(string key, string message);
        void Flush();
    }
}
