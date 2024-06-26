﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace EasySaveUI.Services
{
    abstract class Logger
    {
        protected FileStream Logfile;

        protected void log(string message)
        {
            using (Mutex mutex = new Mutex(false, "Ecriture"))
            {
                mutex.WaitOne();
                // write message to logfile in append mode
                try
                {
                    using (Mutex mutex_lect = new Mutex(false, "Lecture"))
                    {
                        mutex_lect.WaitOne();
                        StreamWriter sw = new StreamWriter(Logfile);
                        sw.WriteLine(message);
                        sw.Close();
                        mutex_lect.ReleaseMutex();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error while writing to log file : " + e.Message);
                }
                mutex.ReleaseMutex();
            }
        }
    }

    class LoggerJournalier : Logger
    {
        private static LoggerJournalier _instance;
        private FileStream _logFileJSON;
        private FileStream _logFileXML;

        private LoggerJournalier()
        {
            OpenFile();
        }

        public static LoggerJournalier GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoggerJournalier();
            }

            return _instance;
        } 

        ~LoggerJournalier()
        {
            Logfile?.Close();
            _logFileJSON?.Close();
            _logFileXML?.Close();
        }

        private void OpenFile()
        {
            DateTime date = DateTime.Now;
            // create a new log file with the current date

            if (_logFileJSON == null || _logFileJSON.Name != "log_" + date.ToString("yyyy-MM-dd") + ".json")
            {
                _logFileJSON?.Close();

                _logFileJSON = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_" + date.ToString("yyyy-MM-dd") + ".json"), FileMode.Append);
            }

            if (_logFileXML == null || _logFileXML.Name != "log_" + date.ToString("yyyy-MM-dd") + ".xml")
            {
                _logFileXML?.Close();

                _logFileXML = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_" + date.ToString("yyyy-MM-dd") + ".xml"), FileMode.Append);
            }
        }

        /// <summary>
        /// Cette methode permet d'ajouter les informations d'un transfer au fichier de log journalier
        /// </summary>
        /// <param name="save_name">Nom de la sauvegarde</param>
        /// <param name="source">Fichier source </param>
        /// <param name="target">Fichier de destination</param>
        /// <param name="size">Taille du fichier en octets</param>
        /// <param name="transfer_time">Temps de transfer millisecondes</param>
        public void Log(string save_name, string source, string target, int size, int? transfer_time , int? encryption_time = null)
        {
            OpenFile();

            /* Enregistrer les logs au format JSON */
            string log_json = "{\n \"Name\": \"" + save_name +
                                "\",\n \"FileSource\": \"" + source +
                                "\",\n \"FileTarget\": \"" + target +
                                "\",\n \"FileSize\": " + size;
                                
                                

            if (encryption_time != null) 
            {
                log_json += ",\n \"Encryption_Time\": " + encryption_time;
            };

            log_json += ",\n \"FileTransferTime\": " + transfer_time +
                        ",\n \"Time\": \"" + DateTime.Now + "\",\n},";

            Logfile = _logFileJSON;

            log(log_json);

            /* Enregistrer les logs au format XML */
            XElement log_xml = 
                new XElement("log",
                    new XElement("Name", save_name),
                    new XElement("FileSource", source),
                    new XElement("FileTarget", target),
                    new XElement("FileSize", size),
                    encryption_time != null ? new XElement("Encryption_Time", encryption_time) : null,
                    new XElement("FileTransferTime", transfer_time),
                    new XElement("Time", DateTime.Now)
                );

            Logfile = _logFileXML;

            log(log_xml.ToString());
        }
    }

    class LoggerEtat : Logger
    {
        private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();
        private static LoggerEtat _instance;

        private LoggerEtat()
        {
            // create a new log file with the current date
            ReopenFile();
        }

        public static LoggerEtat GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoggerEtat();
            }

            return _instance;
        }

        ~LoggerEtat()
        {
            Logfile.Close();
        }

        private void ReopenFile()
        {
            using (Mutex mutex = new Mutex(false, "Ecriture"))
            {
                mutex.WaitOne();
                if (Logfile != null)
                {
                    Logfile.Close();
                }

                Logfile = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.txt"), FileMode.OpenOrCreate);
                mutex.ReleaseMutex();
            }
        }

        public void WriteStatesToFile()
        {
            StringBuilder StatesJSON = new();
            StatesJSON.Append("[\n");

            foreach (Save save in _saveConfiguration.GetConfiguration())
            {
                string state;
                switch (save.State)
                {
                    case SaveState.NOT_STARTED:
                        state = "NOT_STARTED";
                        break;
                    case SaveState.IN_PROGRESS:
                        state = "IN_PROGRESS";
                        break;
                    case SaveState.FINISHED:
                        state = "FINISHED";
                        break;
                    case SaveState.ERROR:
                        state = "ERROR";
                        break;
                    default:
                        state = "UNKNOWN";
                        break;
                }

                StatesJSON.Append("{\n");
                StatesJSON.Append(" \"Name\": \"" + save.Name + "\",\n");
                StatesJSON.Append(" \"SourceFilePath\": \"" + save.InputFolder + "\",\n");
                StatesJSON.Append(" \"TargetFilePath\": \"" + save.OutputFolder + "\",\n");
                StatesJSON.Append(" \"State\": \"" + state + "\",\n");
                StatesJSON.Append(" \"TotalFilesToCopy\": \"" + save.TotalFilesToCopy + "\",\n");
                StatesJSON.Append(" \"TotalFilesSize\": \"" + save.TotalFilesSize + "\",\n");
                StatesJSON.Append(" \"NbFilesLeftToDo\": \"" + save.NbFilesLeftToDo + "\",\n");
                StatesJSON.Append(" }");
            }
            
            StatesJSON.Append("\n]");

            log(StatesJSON.ToString());

            ReopenFile();
        }

    }

    class LoggerConfiguration : Logger
    {
        private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();

        public LoggerConfiguration()
        {
            ReopenFile();
        }

        ~LoggerConfiguration()
        {
            Logfile.Close();
        }

        private void ReopenFile()
        {
            if (Logfile != null)
            {
                Logfile.Close();
            }

            Logfile = new FileStream("config.txt", FileMode.OpenOrCreate);
        }

        public void WriteConfigToFile()
        {
            string config_json = JsonSerializer.Serialize(_saveConfiguration.GetConfiguration());

            log(config_json);
        }
    }
}

