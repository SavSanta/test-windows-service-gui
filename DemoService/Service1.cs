﻿using System.ServiceProcess;
using System.Threading;

namespace DemoService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            Timer t1 = new Timer(AutoStopCallback, null, 15000, -1); // auto stop in 15 seconds for testing
        }

        private void AutoStopCallback(object state)
        {
            Stop();
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(2000);
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            Thread.Sleep(2000);
            base.OnStop();
        }

        protected override void OnContinue()
        {
            Thread.Sleep(2000);
        }

        protected override void OnPause()
        {
            Thread.Sleep(2000);
        }


    }
}
