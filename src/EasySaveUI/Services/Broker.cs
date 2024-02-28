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
                if (client == null)
                {
                    client = listener.AcceptTcpClient();
                }
                writer = new StreamWriter(client.GetStream(), encoding: Encoding.ASCII) { AutoFlush = true };
                listener = new TcpListener(IPAddress.Any, 40000);
                Debug.WriteLine("Client connected");
                NetworkStream stream = client.GetStream();
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
                        foreach (Save save in saveConfiguration.GetConfiguration())
                        {
                            if (save.Id == int.Parse(mess))
                            {
                                saveManager.PauseSave(save);
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
                        foreach (Save save in saveConfiguration.GetConfiguration())
                        {
                            if (save.Id == int.Parse(mess))
                            {
                                saveManager.StopSave(save);
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
                        foreach (Save save in saveConfiguration.GetConfiguration())
                        {
                            if (save.Id == int.Parse(mess))
                            {
                                saveManager.ResumeSave(save);
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
                        foreach (Save save in saveConfiguration.GetConfiguration())
                        {
                            if (save.Id == int.Parse(mess))
                            {
                                saveManager.RunSave(save);
                            }
                        }
                    }
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
    }
}
