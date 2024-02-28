using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;
using System.Xml.Linq;

namespace DistantConsole
{
    internal class Program
    {
        public static List<string> saves = new List<string>();
        public static List<string> saves_statut = new List<string>();
        public static NetworkStream stream = null;
        public static StreamWriter writer = null;
        public static StreamReader reader = null;
        public static List<string> uis = null;


        static void Main(string[] args)
        {
            
            // get input from user
            bool connected = false;
            string ip = "";
            string port = "";
            
            while(!connected)
            {
                Console.WriteLine("Enter the IP address of the server: ");
                ip = Console.ReadLine();
                Console.WriteLine("Enter the port of the server: ");
                port = Console.ReadLine();
                try
                {
                    TcpClient client = new TcpClient(ip, int.Parse(port));
                    connected = true;
                    Console.Clear();
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream) { AutoFlush = true };
                    writer.WriteLine("Client connected");
                    string message = null;
                    uis = new List<string>();
                    uis.Add("---------------------------------");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("---------------------------------");
                    new Thread(console_writer).Start();
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            using (Mutex mutex = new Mutex(false, "Ecriture"))
                            {
                                mutex.WaitOne();
                                string entry;
                                ConsoleKeyInfo keyInfo = Console.ReadKey();
                                switch (keyInfo.Key)
                                {

                                    case ConsoleKey.P:
                                        uis[1] = "Entrez les id des sauvegardes que vous voulez mettre en pause séparés espaces ( all pour tout mettre en pause)";
                                        Console.Clear();
                                        foreach (string ui in uis)
                                        {
                                            Console.WriteLine(ui);
                                        }
                                        foreach (string save in saves)
                                        {
                                            Console.WriteLine(save);
                                        }
                                        entry = Console.ReadLine();
                                        if (entry == "q")
                                        {
                                            break;
                                        }
                                        if (entry == "all")
                                        {
                                            writer.WriteLine("pause all");
                                        }
                                        else
                                        {
                                            List<string> strings = new List<string>();
                                            // split the entry by ;
                                            foreach (string str in strings)
                                            {
                                                writer.WriteLine("pause " + str);
                                            }
                                        }
                                        break;
                                    case ConsoleKey.R:
                                        uis[1] = "Entrez les id des sauvegardes que vous voulez reprendre séparés espaces ( all pour tout reprendre)";
                                        Console.Clear();
                                        foreach (string ui in uis)
                                        {
                                            Console.WriteLine(ui);
                                        }
                                        foreach (string save in saves)
                                        {
                                            Console.WriteLine(save);
                                        }
                                        entry = Console.ReadLine();
                                        if (entry == "q")
                                        {
                                            break;
                                        }
                                        if (entry == "all")
                                        {
                                            writer.WriteLine("resume all");
                                        }
                                        else
                                        {
                                            List<string> strings = new List<string>();
                                            // split the entry by ;
                                            strings = entry.Split(';').ToList();
                                            foreach (string str in strings)
                                            {
                                                writer.WriteLine("resume " + str);
                                            }
                                        }
                                        break;
                                    case ConsoleKey.Q:
                                        uis[1] = "Entrez les id des sauvegardes que vous voulez stopper séparés espaces ( all pour tout stopper)";
                                        Console.Clear();
                                        foreach (string ui in uis)
                                        {
                                            Console.WriteLine(ui);
                                        }
                                        foreach (string save in saves)
                                        {
                                            Console.WriteLine(save);
                                        }
                                        entry = Console.ReadLine();
                                        if (entry == "q")
                                        {
                                            break;
                                        }
                                        if (entry == "all")
                                        {
                                            writer.WriteLine("stop all");
                                        }
                                        else
                                        {
                                            List<string> strings = new List<string>();
                                            // split the entry by ;
                                            strings = entry.Split(';').ToList();
                                            foreach (string str in strings)
                                            {
                                                writer.WriteLine("stop " + str);
                                            }
                                        }
                                        break;
                                    case ConsoleKey.S:
                                        uis[1] = "Entrez les id des sauvegardes que vous voulez lancer séparés espaces ( all pour tout stopper)";
                                        Console.Clear();
                                        foreach (string ui in uis)
                                        {
                                            Console.WriteLine(ui);
                                        }
                                        foreach (string save in saves)
                                        {
                                            Console.WriteLine(save);
                                        }
                                        entry = Console.ReadLine();
                                        if (entry == "q")
                                        {
                                            break;
                                        }
                                        else if (entry == "all")
                                        {
                                            writer.WriteLine("launch all");
                                        }
                                        else
                                        {
                                            List<string> strings = new List<string>();
                                            // split the entry by ;
                                            strings = entry.Split(';').ToList();
                                            foreach (string str in strings)
                                            {
                                                writer.WriteLine("launch " + str);
                                            }
                                        }
                                        break;
                                    case ConsoleKey.L:
                                        writer.WriteLine("list");
                                        message = reader.ReadLine();
                                        List<string> list = message.Split(';').ToList();
                                        Console.Clear();
                                        foreach (string save in list)
                                        {
                                            Console.WriteLine(save);
                                        }
                                        Console.ReadKey();
                                        break;
                                    case ConsoleKey.H:
                                        Console.Clear();
                                        Console.WriteLine("S : Lancer une sauvegarde");
                                        Console.WriteLine("P : Mettre en pause une sauvegarde");
                                        Console.WriteLine("R : Relancer une sauvegarde");
                                        Console.WriteLine("Q : Stopper une sauvegarde");
                                        Console.ReadKey();
                                        break;
                                }
                                Console.Clear();
                                uis[1] = "";
                                foreach (string ui in uis)
                                {
                                    Console.WriteLine(ui);
                                }
                                foreach (string save in saves)
                                {
                                    Console.WriteLine(save);
                                }
                                mutex.ReleaseMutex();
                            }
                        }  
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        public static void console_writer()
        {
            while (true)
            {
                string message = null;
                while (stream.DataAvailable)
                {
                    message = reader.ReadLine();
                
                if (message != null && message.StartsWith("prog_message"))
                {
                    message = message.Replace("prog_message", "");
                    string[] values = message.Split(' ');
                    int files_processed = int.Parse(values[0]);
                    int total_files = int.Parse(values[1]);
                    string name = values[2];
                    int percentage = (int)(((float)files_processed / (float)total_files) * 100);

                    string progress = name + " : " + files_processed + " / " + total_files + "[";

                    for (int i = 0; i < 20; i++)
                    {
                        if (i < percentage / 20 || percentage == 100)
                        {
                            progress += "=";
                        }
                        else if (i == percentage / 20)
                        {
                            progress += ">";
                        }
                        else
                        {
                            progress += " ";
                        }
                    }
                    progress += "] " + percentage + "%";
                    // check if the progress is already in the list
                    bool found = false;
                    for (int i = 0; i < saves.Count; i++)
                    {
                        if (saves[i].Contains(name))
                        {
                            saves[i] = progress;
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        saves.Add(progress);
                    }
                }
                    if (message != null && message.StartsWith("status_message"))
                    {
                        message = message.Replace("status_message", "");
                        string[] values = message.Split(';');
                        string name = values[0];
                        string status = values[1];
                        if(status == "En pause")
                        {
                            Debug.WriteLine("En pause");
                        }
                        bool found = false;
                        for (int i = 0; i < saves_statut.Count; i++)
                        {
                            if (saves_statut[i].Contains(name))
                            {
                                saves_statut[i] = message;
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            saves_statut.Add(message);
                        }
                    }
                }
                using (Mutex mutex = new Mutex(false, "Ecriture"))
                {
                    mutex.WaitOne();
                    Console.Clear();
                    foreach (string ui in uis)
                    {
                        Console.WriteLine(ui);
                    }
                    foreach (string save in saves)
                    {
                        bool found = false;
                        foreach( string sv in saves_statut)
                        {
                            Console.WriteLine(sv);
                            Console.WriteLine(sv.Split(';')[0]);
                            if (save.Contains(sv.Split(';')[0]))
                            {

                                Console.WriteLine(save + sv.Split(';')[1]);
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            Console.WriteLine(save);
                        }
                    }
                    Thread.Sleep(10);
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
