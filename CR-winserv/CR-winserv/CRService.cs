using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using CR_ClassLibrary;

namespace CR_winserv
{
    public partial class CRService : ServiceBase
    {
        public CRService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

        }

        
        public void myCustomTest()
        {
            if (!EventLog.SourceExists("CentralReport Service"))
            {
                EventLog.CreateEventSource("CentralReport Service", "Application");
            }

            EventLog.WriteEntry("CentralReport Service","Launching CR...");

            Collector testCollector = CR_ClassLibrary.Collector.Instance;

            while (true) {
                //EventLog.WriteEntry("CentralReport Service", "CPU Free : " + testCollector.getCPUStats()["Free"].ToString() + "CPU User : " + testCollector.getCPUStats()["User"].ToString() + " - CPU Kernel : " + testCollector.getCPUStats()["Kernel"].ToString());

                Dictionary<string, int> cpuDict = testCollector.getCPUStats();
                Console.WriteLine("CPU Free : " + cpuDict["Free"].ToString() + " - CPU User : " + cpuDict["User"].ToString() + " - CPU Kernel : " + cpuDict["Kernel"].ToString());

                Dictionary<string, int> memoryDict = testCollector.getMemoryStats();
                //Console.WriteLine("Memory Available : " + memoryDict["Available"].ToString());

                List<Dictionary<string, string>> disksList = testCollector.getDisksStats();
                foreach (Dictionary<string, string> oneDiskDict in disksList)
                {
                    Console.WriteLine("Disk : " + oneDiskDict["Name"] + " - Available : " + oneDiskDict["Available"] + " - Total : " + oneDiskDict["Total"]);
                }

                System.Threading.Thread.Sleep(10000);
            }

        }

        protected override void OnStop()
        {
        }
    }
}
