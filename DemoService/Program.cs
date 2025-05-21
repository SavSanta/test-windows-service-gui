using System;
using System.Diagnostics;
using System.Management;
using System.ServiceProcess;
using ServiceProcess.Helpers;

namespace DemoService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };

            //ServiceBase.Run(ServicesToRun);
            ServicesToRun.LoadServices(
                showGuiWhenDebuggerAttached: true,
                showGuiWhenArgumentDetected: true,
                argumentToDetect: "/start",
                startServiceImmediatelyWhenDebuggerAttached: true,
                startServiceImmediatelyWhenArgumentDetected: true
                );
            System.Console.WriteLine();

            // This writes out the current user who is running the process into a TestLog
            // There is a palpable delay when doing this though.
            // THis is helter-skelter for testing my code that needs this.
            // The actual skeleton of this DemoService code is old but robust (aka overengineered) enough for more changes. However, this is what Im going to upload into the repo.
            string usernamedata = GetProcessUser();
            System.IO.File.AppendAllText("C:\\Users\\Public\\Desktop\\TestServiceLog.txt", "UserContext" + usernamedata + "Data Written At Time: " + DateTime.Now.ToString("o") + Environment.NewLine);
            Process.Start("C:\\Windows\\System32\\calc.exe");
        }

        static string GetProcessUser()
        {
            int processId = Process.GetCurrentProcess().Id;

            string query = $"SELECT * FROM Win32_Process WHERE ProcessId = {processId}";
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection results = searcher.Get())
            {
                foreach (ManagementObject mo in results)
                {
                    string[] ownerInfo = new string[2];
                    int returnVal = Convert.ToInt32(mo.InvokeMethod("GetOwner", ownerInfo));

                    if (returnVal == 0)
                    {
                        string user = ownerInfo[0];
                        string domain = ownerInfo[1];
                        return $"Process ID {processId} is owned by: {domain}\\{user}";
                    }
                    else
                    {
                        return "Could not retrieve owner information.";
                    }
                }
            }
            return "failed to get a match on process";
        }

    }

}
