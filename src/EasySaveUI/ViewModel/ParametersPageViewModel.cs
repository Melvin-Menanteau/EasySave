namespace EasySaveUI.ViewModel
{
    public partial class ParametersPageViewModel : BaseViewModel
    {
        private readonly Parameters _parameters = Parameters.GetInstance();

        public ParametersPageViewModel()
        {

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
