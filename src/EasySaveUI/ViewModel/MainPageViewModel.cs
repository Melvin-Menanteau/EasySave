using System.Collections.ObjectModel;

namespace EasySaveUI.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<Save> Saves { get; set; } = new();

    public MainPageViewModel()
    {
        Title = "";
    }

    public void GetSauvegardes()
    {
        Saves = new ObservableCollection<Save>(SaveConfiguration.GetInstance().GetConfiguration());
    }
}