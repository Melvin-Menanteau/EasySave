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
        private StreamReader streamReader;
        private NetworkStream stream;


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
                string message = "";
                if (client == null || !client.Connected)
                {
                    client = listener.AcceptTcpClient();
                
                writer = new StreamWriter(client.GetStream(), encoding: Encoding.ASCII) { AutoFlush = true };
                listener = new TcpListener(IPAddress.Any, 40000);
                Debug.WriteLine("Client connected");
                stream = client.GetStream();
                streamReader = new StreamReader(stream, Encoding.ASCII);
                // if client send message
                SaveManager saveManager = SaveManager.GetInstance();
                if (stream.DataAvailable)
                {
                    message = streamReader.ReadLine();
                }
                if (message.StartsWith("pause"))
                {
                    SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
                    string mess = message.Split(" ")[1];
                    if (mess == "all")
                    {
                        saveManager.PauseAllSaves();
                    }
                    else
                    {
                        foreach (string abc in message.Split(" "))
                        {
                            if (int.TryParse(abc, out _))
                            {
                                foreach (Save save in saveConfiguration.GetConfiguration())
                                {
                                    saveManager.PauseSave(save);
                                }
                            }
                        }
                    }
                }
                else if(message.StartsWith("stop"))
                {
                    SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
                    string mess = message.Split(" ")[1];
                    if (mess == "all")
                    {
                        saveManager.StopAllSaves();
                    }
                    else
                    {
                        foreach (string abc in message.Split(" "))
                        {
                            if (int.TryParse(abc, out _))
                            {
                                foreach (Save save in saveConfiguration.GetConfiguration())
                                {
                                    saveManager.StopSave(save);
                                }
                            }
                        }
                    }
                }
                else if (message.StartsWith("resume"))
                {
                    SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
                    string mess = message.Split(" ")[1];
                    if (mess == "all")
                    {
                        saveManager.ResumeAllSaves();
                    }
                    else
                    {
                        foreach (string abc in message.Split(" "))
                        {
                            if (int.TryParse(abc, out _))
                            {
                                foreach (Save save in saveConfiguration.GetConfiguration())
                                {
                                    saveManager.ResumeSave(save);
                                }
                            }
                        }
                    }
                }
                else if (message.StartsWith("launch"))
                {
                    SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
                    string mess = message.Split(" ")[1];
                    if (mess == "all")
                    {
                        
                        foreach (Save save in saveConfiguration.GetConfiguration())
                        {
                            saveManager.RunSave(save);
                        }
                    }
                    else
                    {

                        foreach (string abc in message.Split(" "))
                        {
                            if (int.TryParse(abc, out _))
                            {
                                foreach (Save save in saveConfiguration.GetConfiguration())
                                {

                                    // check if mess contain uniquely int value
                                    if (save.Id == int.Parse(abc))
                                    {
                                        saveManager.RunSave(save);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (message.StartsWith("list"))
                {
                    SaveConfiguration saveConfiguration = SaveConfiguration.GetInstance();
                    string messagelist = "";
                    foreach (Save save in saveConfiguration.GetConfiguration())
                    {
                        messagelist += "ID:" + save.Id + " Name: " + save.Name + ";";
                    }
                    SendToClient(messagelist);
                }
                }
                //Thread.Sleep(500);
                if (!client.Connected)
                {
                    client = null;
                }
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

        public void SendStatusToClient(string name, string status)
        {
            using (Mutex mutex = new Mutex(false, "EasySaveUIBrokerMutex"))
            {
                string message = "status_message" + name + ";" + status;
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
