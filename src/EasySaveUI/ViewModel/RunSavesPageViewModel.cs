using System.Collections.ObjectModel;

namespace EasySaveUI.ViewModel
{
    public partial class RunSavesPageViewModel : BaseViewModel
    {

        private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();
        public ObservableCollection<Save> Saves { get; set; } = new();

        public RunSavesPageViewModel()
        {
            Title = "";
        }

        public void GetSauvegardes()
        {
            Saves = new ObservableCollection<Save>(_saveConfiguration.GetConfiguration());
        }

    }
}
