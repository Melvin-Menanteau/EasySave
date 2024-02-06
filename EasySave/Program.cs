using System;
using System.Collections.Generic;

namespace EasySave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
                Console.ResetColor();

                var commande = Console.ReadLine();

                if (commande == string.Empty) 
                {
                    continue;
                }
                else if (commande.StartsWith("ls"))
                {
                    EasySave easySaveC = new EasySave();
                    SaveConfiguration saveConfiguration;
                    saveConfiguration = SaveConfiguration.GetInstance();
                    List<int> ids = [];
                    List<Save> saves = saveConfiguration.GetConfiguration();
                    foreach (Save save in saves)
                    {
                        ids.Add(save.Id);
                    }
                    easySaveC.LancerSauvegarde(ids);
                }
                else
                {
                    GetMethodFromCommand getMethodFromCommand = new();
                    getMethodFromCommand.GetMethod(commande);
                }
            }
        }
    }
}
