using System;

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
                else
                {
                    CommandInterpretor.ReadCommand(commande);
                }
            }
        }
    }
}
