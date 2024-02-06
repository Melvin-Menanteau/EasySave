using System;
using System.Collections.Generic;

namespace EasySave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SharedLocalizer sharedLocalizer = SharedLocalizer.GetInstance();

            Console.WriteLine(sharedLocalizer.GetLocalizedString("Hello"));
            sharedLocalizer.SetCulture(new System.Globalization.CultureInfo("en-EN"));
            Console.WriteLine(sharedLocalizer.GetLocalizedString("Hello"));

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
                else
                {
                    GetMethodFromCommand getMethodFromCommand = new GetMethodFromCommand();
                    getMethodFromCommand.GetMethod(commande);
                }
            }
        }
    }
}
