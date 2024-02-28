using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EasySaveUI.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly SaveConfiguration _saveConfiguration = SaveConfiguration.GetInstance();
    private SaveManager _saveManager;
    public ObservableCollection<Save> Saves { get; set; } = [];
    public Save SelectedSave { get; set; }

    public MainPageViewModel()
    {
        Title = "";
        _saveManager = SaveManager.GetInstance();
    }

    public void GetSauvegardes()
    {
        Saves = new ObservableCollection<Save>(_saveConfiguration.GetConfiguration());
    }

    public void SetSelectedSave(Save save)
    {
        SelectedSave = save;

        if (save != null)
            _saveConfiguration.SetSelectedSave(save.Id);
        else
            _saveConfiguration.SetSelectedSave(null);
    }

    public void AddSave(string name, string inputFolder, string outputFolder, SaveType saveType)
    {
        _saveConfiguration.AddConfiguration(name, inputFolder, outputFolder, saveType);
    }

    public void UpdateSave(string name, string inputFolder, string outputFolder, SaveType saveType)
    {
        _saveConfiguration.UpdateConfiguration(SelectedSave.Id, name, inputFolder, outputFolder, saveType);
    }

    public void RemoveSave()
    {
        _saveConfiguration.RemoveConfiguration(SelectedSave.Id);
    }

    public void TogglePauseSave(Save save)
    {
        if (save.State == SaveState.PAUSED)
        {
            _saveManager.ResumeSave(save);
        }
        else if (save.State == SaveState.IN_PROGRESS)
        {
            _saveManager.PauseSave(save);
        }
    }

    public void StopSave(Save save)
    {
        if (save.State == SaveState.IN_PROGRESS || save.State == SaveState.PAUSED)
        {
            _saveManager.StopSave(save);
        }
    }
}