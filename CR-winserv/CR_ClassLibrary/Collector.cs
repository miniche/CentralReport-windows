using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;

namespace CR_ClassLibrary
{
    public class Collector
    {

        // Singleton
        private static Collector _collectorInstance;

        // Dictionaries
        private Dictionary<string, int> cpuDict { get; set; }
        private Dictionary<string, int> memoryDict { get; set; }
        private List<Dictionary<string, string>> disksList { get; set; }

        // Performance counters
        private System.Diagnostics.PerformanceCounter cpuCounterUser { get; set; }
        private System.Diagnostics.PerformanceCounter cpuCounterKernel { get; set; }
        private System.Diagnostics.PerformanceCounter memoryCounterAvailable { get; set; }



        private Collector() {
            // CPU Checks
            this.cpuDict = new Dictionary<string, int>();
            this.cpuCounterUser = new System.Diagnostics.PerformanceCounter("Processor", "% Privileged Time", "_Total");
            this.cpuCounterKernel = new System.Diagnostics.PerformanceCounter("Processor", "% User Time", "_Total");

            // Memory checks
            this.memoryDict = new Dictionary<string, int>();
            this.memoryCounterAvailable = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes", "");

            // Disks checks
            this.disksList = new List<Dictionary<string, string>>();
        }


        public static Collector Instance
        {
            get 
            {
                if (_collectorInstance == null)
                {
                    _collectorInstance = new Collector();
                }

                 return _collectorInstance;
            }
        }


        public Dictionary<string, int> getCPUStats()
        {
            this.cpuDict.Clear();
            
            this.cpuDict.Add("User", (int) Math.Floor(cpuCounterUser.NextValue()));
            this.cpuDict.Add("Kernel", (int) Math.Floor(cpuCounterKernel.NextValue()));
            this.cpuDict.Add("Free", 100 - this.cpuDict["User"] - this.cpuDict["Kernel"]);

            return this.cpuDict;

        }

        public Dictionary<string, int> getMemoryStats()
        {
            this.memoryDict.Clear();

            // Read http://msdn.microsoft.com/en-us/library/windows/desktop/aa394239(v=vs.85).aspx for all available keys
            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);
            ManagementObjectCollection getObjects = searcher.Get();

            foreach (ManagementObject item in getObjects)
            {
                Console.WriteLine("FreeVirtualMemory = " + item["FreeVirtualMemory"]);
                Console.WriteLine("FreePhysicalMemory = " + item["FreePhysicalMemory"]);
                Console.WriteLine("TotalVirtualMemorySize = " + item["TotalVirtualMemorySize"]);
                Console.WriteLine("TotalVisibleMemorySize = " + item["TotalVisibleMemorySize"]);
            }

            this.memoryDict.Add("Available", (int)Math.Floor(memoryCounterAvailable.NextValue()));

            return memoryDict;
        }

        public List<Dictionary<string,string>> getDisksStats()
        {
            this.disksList.Clear();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Dictionary<string, string> oneDiskDict = new Dictionary<string, string>();

                if (d.IsReady == true)
                {
                    oneDiskDict.Add("Name", d.Name);
                    oneDiskDict.Add("Type", d.DriveType.ToString());

                    oneDiskDict.Add("Available", d.TotalFreeSpace.ToString());
                    oneDiskDict.Add("Total", d.TotalSize.ToString());

                    disksList.Add(oneDiskDict);
                }  
            }

            return disksList;
        }
    }
}
