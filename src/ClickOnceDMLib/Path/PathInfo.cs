﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace ClickOnceDMLib.Path
{
    public abstract class PathInfoBase
    {
        public static string workspace = ConfigurationManager.AppSettings["Workspace"];

        public static string EnsurePath(string path)
        {
            path = System.IO.Path.Combine(workspace, path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }

    public class PathInfo : PathInfoBase
    {
        public static string GetTicketFile(string fileName)
        {
            return System.IO.Path.Combine(Ticket, fileName);
        }

        public static string Ticket
        {
            get
            {
                return EnsurePath("Ticket");
            }
        }

        public static string Queue
        {
            get
            {
                return EnsurePath("Queue");
            }
        }


        public static string Complete
        {
            get
            {
                return EnsurePath("Complete");
            }
        }

        public static string Bad
        {
            get
            {
                return EnsurePath("Bad");
            }
        }

        public static string Log
        {
            get
            {
                return EnsurePath("Log");
            }
        }
    }
}
