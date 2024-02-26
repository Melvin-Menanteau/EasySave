using System.Net.Sockets;

namespace DistantConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                                if (i < percentage % 20 - 1)
                                {
                                    progress += "=";
                                }
                                else if (i == percentage % 20 - 1)
                                {
                                    progress += ">";
                                }
                                else
                                {
                                    progress += " ";
                                }
                            }
                            progress += "] " + percentage + "%";
                            Console.WriteLine(progress);


                        }
                        else
                        {
                            Console.WriteLine(message);
                        }

                        // TODO: ajouter une methode pour mettre en forme le messag recu. 
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
