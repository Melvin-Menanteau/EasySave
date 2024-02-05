using System;
using System.Collections.Generic;
using System.IO;

namespace EasySave
{
    abstract class Logger
    {
        protected FileStream Logfile;
        public void log(string message)
        {
            // write message to logfile in append mode
            try
            {
                StreamWriter sw = new StreamWriter(Logfile);
                sw.WriteLine(message);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while writing to log file : " + e.Message);
            }
        }
    }

    class LoggerJournalier : Logger
    {
        public LoggerJournalier()
        {
            // get current date
            DateTime date = DateTime.Now;
            // create a new log file with the current date
            Logfile = new FileStream("log_" + date.ToString("yyyy-MM-dd") + ".txt", FileMode.Append);
        }

        ~LoggerJournalier()
        {
            Logfile.Close();
        }



        /// <summary>
        /// Cette methode permet d'ajouter les informations d'un transfer au fichier de log journalier
        /// </summary>
        /// <param name="save_name">Nom de la sauvegarde</param>
        /// <param name="source">Fichier source </param>
        /// <param name="target">Fichier de destination</param>
        /// <param name="size">Taille du fichier en octets</param>
        /// <param name="transfer_time">Temps de transfer millisecondes</param>
        public void Log(string save_name, string source, string target, int size, float transfer_time)
        {
            // get current date hour
            DateTime date = DateTime.Now;
            // write the date hour and the message to the log file
            string datetime = date.ToString("yyyy-MM-dd HH:mm:ss");

            // get filename of Logfile to check if the date has changed
            string filename = Logfile.Name;
            if (filename != "log_" + date.ToString("yyyy-MM-dd") + ".txt")
            {
                // close the current log file
                Logfile.Close();
                // create a new log file with the current date
                Logfile = new FileStream("log_" + date.ToString("yyyy-MM-dd") + ".txt", FileMode.Append);
            }

            string log_json = "{\n \"Name\": \"" + save_name + "\",\n \"FileSource\": \"" + source + "\",\n \"FileTarget\": \"" + target + "\",\n \"FileSize\": " + size + ",\n \"FileTransferTime\": " + transfer_time + "\",\n \"Time\": \"" + date + "\",\n},";
            log(log_json);
        }
    }

    class LoggerEtat : Logger
    {
        List<Save> saves;
        public LoggerEtat()
        {
            // create a new log file with the current date
            Logfile = new FileStream("state.txt", FileMode.Append);
        }
        ~LoggerEtat()
        {
            Logfile.Close();
        }
        public void AddSave(Save save)
        {
            saves.Add(save);
        }

    }

}

