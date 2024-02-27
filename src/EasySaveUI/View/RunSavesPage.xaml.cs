using System.Resources;

namespace EasySaveUI.View;

public partial class RunSavesPage : ContentPage
{
    RunSavesPageViewModel viewModel;
    List<Save> SavesSelected = new List<Save>();
    private ResourceManager _resourceManager;
    private bool returnPressed = false;

    public RunSavesPage(RunSavesPageViewModel viewModel)
	{
		InitializeComponent();
        this.viewModel = viewModel;
        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);
        MessagingCenter.Subscribe<LanguesSettingsView>(this, "LanguageChanged", (sender) =>
        {
            LoadLocalizedTexts();
        });
        LoadLocalizedTexts();
    }

    private void LoadLocalizedTexts()
    {
        var cultureInfo = App.LanguageService.CurrentLanguage;
        TitreLancementSauvegarde.Text = _resourceManager.GetString("TitreLancementSauvegardeKey", cultureInfo);
        SavesToRunLabel.Text = _resourceManager.GetString("SavesToRunLabelKey", cultureInfo);
        RunSavesButton.Text = _resourceManager.GetString("RunSavesButtonKey", cultureInfo);
    }

    protected override void OnAppearing()
    {
        viewModel.GetSauvegardes();

        SavesSelectedCollection.ItemsSource = viewModel.Saves;

        // Fait apparaitre la page principale
        base.OnAppearing();
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        if (!returnPressed)
        {
            await Shell.Current.GoToAsync("../", false);
        }
        returnPressed = true;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;

        Save Save = viewModel.Saves.FirstOrDefault(x => x.Id == (checkBox.BindingContext as Save).Id);

        if (checkBox.IsChecked)
        {
            SavesSelected.Add(Save);
        }
        else
        {
            SavesSelected.Remove(Save);
        }

        EditorSaveEditor();
    }

    private void EditorSaveEditor()
    {
        SavesEditor.Text = string.Empty;

        SavesEditor.Text = String.Join("\n", SavesSelected.Select((save) => $"{save.Name} (id: {save.Id})"));
    }

    private async void RunSavesButton_Clicked(object sender, EventArgs e)
    {
        SaveManager saveManager = SaveManager.GetInstance();

        SavesSelected.ForEach(saveManager.RunSave);

        SavesSelected.Clear();

        await Shell.Current.GoToAsync("../", false);
    }
}