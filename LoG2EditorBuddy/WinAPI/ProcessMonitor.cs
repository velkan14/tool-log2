using Log2CyclePrototype.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace Log2CyclePrototype.WinAPI
{
    class ProcessMonitor
    {

        public static event EventHandler FoundLoG2Process;


        string ComputerName = "localhost";
        string WmiQuery;
        ManagementEventWatcher Watcher;
        ManagementScope Scope;

        private void WmiEventHandler(object sender, EventArrivedEventArgs e)
        {
            //in this point the new events arrives
            //you can access to any property of the Win32_Process class
            string procName = (string)((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Name"];
            string pid = (string)((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Handle"];

            if (procName.Contains("grimrock2"))
            {
                Logger.AppendText("Found LoG2 process with PID " + pid);
                Logger.AppendText(StringResources.PickDirString);
                MainForm.LoG2ProcessFound = true;
                //FoundLoG2Process(null, null);
                StopWatcher();
            }
        }

        public ProcessMonitor()
        {
            try
            {
                Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);
                Scope.Connect();

                WmiQuery = "Select * From __InstanceCreationEvent Within 1 " +
                "Where TargetInstance ISA 'Win32_Process' ";

                Watcher = new ManagementEventWatcher(Scope, new EventQuery(WmiQuery));
                Watcher.EventArrived += new EventArrivedEventHandler(this.WmiEventHandler);
                //Watcher.Start();
                //Console.Read();
                //Watcher.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0} Trace {1}", e.Message, e.StackTrace);
            }

        }

        public void StartWatcher()
        {
            try
            {
                Watcher.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void StopWatcher()
        {
            try
            {
                Watcher.Stop();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
