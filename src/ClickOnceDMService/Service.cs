using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ClickOnceDMService
{
    public partial class Service : ServiceBase
    {
        private QueueService queueService = new QueueService();
        private SendService sendService = new SendService();

        private System.Timers.Timer timer;
        private Object lockObj = new Object();

        public Service()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(5000);
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            LogProcess.Info("Start");

            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            LogProcess.Info("Stop");

            timer.Enabled = false;
            timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            IProcess[] threads = { queueService, sendService };

            foreach (IProcess thread in threads)
            {
                new Thread(new ThreadStart(thread.Process)).Start();
            }
        }
    }
}