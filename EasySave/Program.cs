using System;
using System.Collections.Generic;

namespace EasySave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
            saveConfiguration.AddConfiguration("math", "/i/i", "/i/i", SaveType.COMPLETE);
            saveConfiguration.SaveConfigToFile();
        }
    }
}
