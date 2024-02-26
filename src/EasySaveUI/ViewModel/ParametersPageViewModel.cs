namespace EasySaveUI.ViewModel
{
    public partial class ParametersPageViewModel : BaseViewModel
    {
        private readonly Parameters _parameters = Parameters.GetInstance();

        public ParametersPageViewModel()
        {

        }

        public void SaveExtension(List<string> extentionList)
        {
            _parameters.ExtensionsList = extentionList;
            _parameters.WriteJsonExtensionFile();
        }

        public List<string> GetExtensionList()
        {
            _parameters.ReadJsonExtensionFile();
            return _parameters.ExtensionsList;
        }
    }
}
