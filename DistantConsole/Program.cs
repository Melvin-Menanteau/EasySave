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
                        Console.WriteLine(message);
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
