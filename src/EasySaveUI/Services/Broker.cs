using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace EasySaveUI.Services
{
    internal class Broker
    {

        private static Broker _instance;
        private TcpListener listener;
        private TcpClient client = null;
        private StreamWriter writer;


        private Broker()
        {
            listener = new TcpListener(IPAddress.Any , 40000);
            listener.Start();
        }

        public static Broker GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Broker();
            }

            return _instance;
        }

        public void Brok()
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                writer = new StreamWriter(client.GetStream(), encoding: Encoding.ASCII) { AutoFlush = true };
                Debug.WriteLine("Client connected");
                // send message to client
            }
        }

        public void SendToClient(string message)
        {
            using (Mutex mutex = new Mutex(false, "EasySaveUIBrokerMutex"))
            {
                mutex.WaitOne();
                if (client != null)
                {
                    writer.WriteLine(message);
                }
                mutex.ReleaseMutex();
            }
        }


        public void SendProgressToClient(string name ,int files_processed  ,int nb_total)
        {
            using (Mutex mutex = new Mutex(false, "EasySaveUIBrokerMutex"))
            {
                string message = "prog_message" + files_processed + " " + nb_total + " " + name;
                mutex.WaitOne();
                if (client != null)
                {
                    writer.WriteLine(message);
                }
                mutex.ReleaseMutex();
            }
        }
    }
}
