using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasySaveUI.ViewModel
{
    public partial class ParametersPageViewModel : BaseViewModel
    {
        private readonly Parameters _parameters = Parameters.GetInstance();
        public List<string> XmlLogEntries { get; private set; }
        public List<string> JsonLogEntries { get; private set; }
        public List<string> StatusLogEntries { get; private set; }

        public ParametersPageViewModel()
        {
            LoadXmlLogEntries();
            LoadJsonLogEntries();
            LoadStatusLogEntries();
        }

        public List<string> LoadStatusLogEntries()
        {
            DateTime date = DateTime.Now;
            List<string> statusLogs = new List<string>();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "state.txt");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        statusLogs.Add(line);
                    }
                }
            }

            return statusLogs;
        }
        public List<string> LoadJsonLogEntries()
        {
            DateTime date = DateTime.Now;
            List<string> jsonLogs = new List<string>();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_" + date.ToString("yyyy-MM-dd") + ".json");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        jsonLogs.Add(line);
                    }
                }
            }

            return jsonLogs;
        }
        public List<string> LoadXmlLogEntries()
        {
            DateTime date = DateTime.Now;
            List<string> xmlLogs = new List<string>();
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log_" + date.ToString("yyyy-MM-dd") + ".xml");

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        xmlLogs.Add(line);
                    }
                }
            }

            return xmlLogs;
        }
        public void SaveBusinessApplications(List<string> businessApplicationsList)
        {
            _parameters.BusinessApplicationsList = businessApplicationsList;
            _parameters.WriteJsonBusinessApplicationsFile();
        }

        public List<string> GetBusinessApplicationsList()
        {
            _parameters.ReadJsonBusinessApplicationsFile();
            return _parameters.BusinessApplicationsList;
        }

        public void SaveMaxFileSize(int maxFileSize)
        {
            _parameters.MaxFileSize = maxFileSize;
            _parameters.WriteMaxFileSize();
        }
        public int GetMaxFileSize()
        {
            _parameters.ReadMaxFileSize();
            return _parameters.MaxFileSize;
        }

        public void SavePriorityExtension(List<string> extentionList)
        {
            _parameters.PriorityExtensionsList = extentionList;
            _parameters.WriteJsonPriorityExtensionFile();
        }

        public List<string> GetPriorityExtensionList()
        {
            _parameters.ReadJsonPriorityExtensionFile();
            return _parameters.PriorityExtensionsList;
        }

        public void SaveEncryptionExtension(List<string> extentionList)
        {
            _parameters.EncryptionExstensionsList = extentionList;
            _parameters.WriteJsonEncryptionExtensionFile();
        }

        public List<string> GetEncryptionExtensionList()
        {
            _parameters.ReadJsonEncryptionExtensionFile();
            return _parameters.EncryptionExstensionsList;
        }

        public void SaveEncryptionKey(string key)
        {
            _parameters.EncryptionKey = key;
            _parameters.WriteEncryptionKey(key);
        }

        public string GetEncryptionKey()
        {
            return _parameters.ReadEncryptionKey();
        }
    }
}
