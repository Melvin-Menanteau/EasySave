using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveUI.Services
{
    internal class BusinessObserver
    {
        void Observer(string business)
        {
            var processes = Process.GetProcessesByName(business);
            while (true)
            {
                processes = Process.GetProcessesByName(business);
                if (processes.Length > 0)
                {
                    using (Mutex mutex = new Mutex(true, "EasySave", out bool createdNew))
                    {
                        mutex.WaitOne(); 
                        while(processes.Length > 0)
                        {
                            processes = Process.GetProcessesByName(business);
                        }
                        mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}
