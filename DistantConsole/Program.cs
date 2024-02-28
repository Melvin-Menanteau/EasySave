using System.Net.Sockets;
using System.Numerics;

namespace DistantConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> saves = new List<string>();

            // get input from user
            bool connected = false;
            while(!connected)
            {
                Console.WriteLine("Enter the IP address of the server: ");
                string ip = Console.ReadLine();
                Console.WriteLine("Enter the port of the server: ");
                string port = Console.ReadLine();
                try
                {
                    TcpClient client = new TcpClient(ip, int.Parse(port));
                    connected = true;
                    Console.Clear();
                    NetworkStream stream = client.GetStream();
                    StreamReader reader = new StreamReader(stream);
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                    writer.WriteLine("Client connected");
                    while (true)
                    {
                        string message = reader.ReadLine();
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
    }
}
