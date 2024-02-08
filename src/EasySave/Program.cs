using System;

namespace EasySave
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("> ");
                Console.ResetColor();

                var commande = Console.ReadLine();

                if (commande == "exit")
                {
                    break;
                }

                if (commande != string.Empty) 
                {
                    try
                    {
                        CommandInterpretor.ReadCommand(commande);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

        }
    }
}
