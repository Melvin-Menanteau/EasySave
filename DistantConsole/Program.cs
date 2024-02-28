using System.Diagnostics;
using System.Net.Sockets;
using System.Numerics;

namespace DistantConsole
{
    internal class Program
    {
        public static List<string> saves = new List<string>();
        public static NetworkStream stream = null;
        public static StreamReader reader = null;


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
                    NetworkStream stream = client.GetStream();
                    reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                    writer.WriteLine("Client connected");
                    string message = null;
                    List<string> uis = new List<string>();
                    uis.Add("---------------------------------");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("");
                    uis.Add("---------------------------------");
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            string entry;
                            ConsoleKeyInfo keyInfo = Console.ReadKey();
                            switch(keyInfo.Key)
                            {

                                case ConsoleKey.P:
                                    uis[1] = "Entrez les id des sauvegardes que vous voulez mettre en pause ( all pour tout mettre en pause)";
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
                                    if( entry == "q")
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
                                    uis[1] = "Entrez les id des sauvegardes que vous voulez reprendre ( all pour tout reprendre)";
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
                                case ConsoleKey.S:
                                    uis[1] = "Entrez les id des sauvegardes que vous voulez stopper ( all pour tout stopper)";
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
                                case ConsoleKey.L:
                                    uis[1] = "Entrez les id des sauvegardes que vous voulez lancer ( all pour tout stopper)";
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

                                    if (entry == "q" || entry == "" || entry ==" ")
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
                        }
                        if (stream.DataAvailable)
                        {
                            message = reader.ReadLine();
                        }
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
                            Console.Clear();
                            foreach(string ui in uis)
                            {
                                Console.WriteLine(ui);
                            }
                            foreach (string save in saves)
                            {
                                Console.WriteLine(save);
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

        public static void observer(bool play)
        {
            while (play)
            {
                string message = "";
                if (stream.DataAvailable)
                {
                    message = reader.ReadLine();
                }
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
            }
        }

    }
}
