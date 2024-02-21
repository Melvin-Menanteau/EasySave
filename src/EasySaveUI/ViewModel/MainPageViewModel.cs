using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EasySaveUI.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();
    public ObservableCollection<Save> Saves { get; set; } = new();
    public Save SelectedSave { get; set; }

    public MainPageViewModel()
    {
        Title = "";
    }

    public void GetSauvegardes()
    {
        Saves = new ObservableCollection<Save>(_saveConfiguration.GetConfiguration());
    }

    public void SetSelectedSave(Save save)
    {
        SelectedSave = save;
        _saveConfiguration.SetSelectedSave(save.Id);
        Debug.WriteLine($"Selected save: {save.Name}");
    }

    public void AddSave(string name, string inputFolder, string outputFolder, SaveType saveType)
    {
        _saveConfiguration.AddConfiguration(name, inputFolder, outputFolder, saveType);
        GetSauvegardes();
    }
}