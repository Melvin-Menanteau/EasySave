namespace EasySaveUI.ViewModel
{
    public partial class ParametersPageViewModel : BaseViewModel
    {
        private readonly Parameters _parameters = Parameters.GetInstance();
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public List<string> XmlLogEntries { get; private set; }
        public List<string> JsonLogEntries { get; private set; }
        public List<string> StatusLogEntries { get; private set; }

        public ParametersPageViewModel()
        {
            LoadXmlLogEntries();
        }

        public List<string> LoadStatusLogEntries()
        {
            List<string> statusLogs = new List<string>();
            if (File.Exists(currentDirectory + "state.txt"))
            {
                string[] lines = File.ReadAllLines(currentDirectory + "state.txt");
                statusLogs.AddRange(lines);

                return statusLogs;
            }

            return statusLogs;
        }
        private List<string> LoadJsonLogEntries()
        {
            List<string> jsonLogs = new List<string>();
            if (File.Exists(currentDirectory + "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json"))
            {
                string[] lines = File.ReadAllLines(currentDirectory + "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json");
                jsonLogs.AddRange(lines);

                return jsonLogs;
            }

            return jsonLogs;
        }
        private List<string> LoadXmlLogEntries()
        {
            List<string> xmlLogs = new List<string>();
            if (File.Exists(currentDirectory + "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml"))
            {
                string[] lines = File.ReadAllLines(currentDirectory + "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml");
                xmlLogs.AddRange(lines);

                return xmlLogs;
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
