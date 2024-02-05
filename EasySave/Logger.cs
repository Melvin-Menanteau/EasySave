using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EasySave
{
    abstract class Logger
    {
        protected FileStream Logfile;
        public void log(string message) {
            // write message to logfile in append mode
            try
            {
                StreamWriter sw = new StreamWriter(Logfile);
                sw.WriteLine( message);
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

        public void Log(string save_name ,string source , string target , int size , float transfer_time)
        {
            // get current date hour
            DateTime date = DateTime.Now;
            // write the date hour and the message to the log file
            string datetime = date.ToString("yyyy-MM-dd HH:mm:ss");

            // get filename of Logfile to check if the date has changed
            string filename = Logfile.Name;
            if(filename != "log_" + date.ToString("yyyy-MM-dd") + ".txt")
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


}

