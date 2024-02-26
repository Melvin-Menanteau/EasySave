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
        public BusinessObserver() { }

        public static void Observer(string business)
        {
            var processes = Process.GetProcessesByName(business);
            while (true)
            {
                processes = Process.GetProcessesByName(business);
                if (processes.Length > 0)
                {
                    while(processes.Length > 0)
                    {
                        processes = Process.GetProcessesByName(business);
                        Debug.WriteLine("Process " + business + " is running");
                        SaveManager sm = SaveManager.GetInstance();
                        sm.StopAllSaves();
                    }
                }
            }
        }

    }
}
