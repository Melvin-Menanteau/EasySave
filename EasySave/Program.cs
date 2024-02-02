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

                var variable = Console.ReadLine();

                if (variable == string.Empty) 
                {
                    continue;
                }
                else if (variable.StartsWith("cs"))
                {
                    Console.WriteLine("Création d'une sauvegarde");
                }
                else if (variable.StartsWith("ls"))
                {
                    Console.WriteLine("Lancement d'une sauvegarde");
                }
                else if (variable.StartsWith("cls"))
                {
                    Console.Clear();
                }
            }
        }
    }
}
