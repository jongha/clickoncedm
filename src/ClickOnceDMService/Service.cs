using ClickOnceDMLib.Process;
using ClickOnceDMLib.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ClickOnceDMService
{
    public partial class Service : ServiceBase
    {
        private System.Timers.Timer timer;

        public Service()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(10000);
            timer.Enabled = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            timer.Stop();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Mutex mutex = new Mutex();
            mutex.WaitOne();

            QueueProcess queueProcess = new QueueProcess();
            SendMailProcess sendMail = new SendMailProcess();

            List<Queue> queues = queueProcess.GetQueue();
            foreach (Queue queue in queues)
            {
                //sendMail.Send(
                //    new System.Net.Mail.MailAddress()
                //    )
            }

            mutex.ReleaseMutex();

            //System.Console.WriteLine("timer");
        }
    }
}
