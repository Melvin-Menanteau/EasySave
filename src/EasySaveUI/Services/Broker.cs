using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace EasySaveUI.Services
{
    internal class Broker
    {
        public enum StateEnum
        {
            STOPPED,
            RUNNING,
            PAUSED
        }

        private TcpListener listener;
        private TcpClient client = null;
        private StateEnum state = StateEnum.STOPPED;
        private StreamWriter writer;


        public Broker()
        {
            listener = new TcpListener(IPAddress.Any , 40000);
            listener.Start();
        }

        public void Brok()
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                writer = new StreamWriter(client.GetStream(), encoding: Encoding.ASCII) { AutoFlush = true };
                Debug.WriteLine("Client connected");
                // send message to client
                writer.WriteLine("Server connected");
                SendProgressToClient("test", 1, 2);
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
