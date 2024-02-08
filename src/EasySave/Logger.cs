using System;
using System.IO;
using System.Text;

namespace EasySave
{
    abstract class Logger
    {
        protected FileStream Logfile;
        protected void log(string message)
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

            // get filename of Logfile to check if the date has changed
            string filename = Logfile.Name;
            if (filename != "log_" + date.ToString("yyyy-MM-dd") + ".txt")
            {
                // close the current log file
                Logfile.Close();
                // create a new log file with the current date
                Logfile = new FileStream("log_" + date.ToString("yyyy-MM-dd") + ".txt", FileMode.Append);
            }

            string log_json = "{\n \"Name\": \"" + save_name + "\",\n \"FileSource\": \"" + source + "\",\n \"FileTarget\": \"" + target + "\",\n \"FileSize\": " + size + ",\n \"FileTransferTime\": " + transfer_time + ",\n \"Time\": \"" + date + "\",\n},";
            log(log_json);
        }
    }

    class LoggerEtat : Logger
    {
        private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();

        public LoggerEtat()
        {
            // create a new log file with the current date
            reopenFile();
        }
        ~LoggerEtat()
        {
            Logfile.Close();
        }

        private void reopenFile()
        {
            if (Logfile != null)
            {
                Logfile.Close();
            }

            Logfile = new FileStream("state.txt", FileMode.OpenOrCreate);
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

                float progress = 1 - save.TotalFilesToCopy / (save.NbFilesLeftToDo != 0 ? save.NbFilesLeftToDo : 1);

                StatesJSON.Append("{\n");
                StatesJSON.Append(" \"Name\": \"" + save.Name + "\",\n");
                StatesJSON.Append(" \"SourceFilePath\": \"" + save.InputFolder + "\",\n");
                StatesJSON.Append(" \"TargetFilePath\": \"" + save.OutputFolder + "\",\n");
                StatesJSON.Append(" \"State\": \"" + state + "\",\n");
                StatesJSON.Append(" \"TotalFilesToCopy\": \"" + save.TotalFilesToCopy + "\",\n");
                StatesJSON.Append(" \"TotalFilesSize\": \"" + save.TotalFilesSize + "\",\n");
                StatesJSON.Append(" \"NbFilesLeftToDo\": \"" + save.NbFilesLeftToDo + "\",\n");
                StatesJSON.Append(" \"Progression\": \"" + progress + "\",\n");
                StatesJSON.Append(" }");
            }
            
            StatesJSON.Append("\n]");

            log(StatesJSON.ToString());

            reopenFile();
        }

    }

}

