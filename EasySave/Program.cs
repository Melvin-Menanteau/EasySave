using System;
using System.Collections.Generic;

namespace EasySave
{
    public class Program
    {
        public string hello()
        {
            return "Hello";
        }

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
                else
                {
                    GetMethodFromCommand getMethodFromCommand = new GetMethodFromCommand();
                    getMethodFromCommand.GetMethod(commande);
                }
            }
        }
    }
}
