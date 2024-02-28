namespace EasySaveUI.View;
using CommunityToolkit.Maui.Storage;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

public partial class MainPage : ContentPage
{
    MainPageViewModel viewModel;
    private ResourceManager _resourceManager;
    public bool IsNew = false;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        FormulaireConfiguration.IsVisible = false;

        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);
        MessagingCenter.Subscribe<LanguesSettingsView>(this, "LanguageChanged", (sender) =>
        {
            LoadLocalizedTexts();
        });
        LoadLocalizedTexts();
        foreach (var saveType in Enum.GetValues(typeof(SaveType)))
        {
            EntrySaveType.Items.Add(saveType.ToString());
        }
    }

    private void LoadLocalizedTexts()
    {
        var cultureInfo = App.LanguageService.CurrentLanguage;
        TitleConfigurationLabel.Text = _resourceManager.GetString("TitleConfigurationLabelKey", cultureInfo);
        SaveNameLabel.Text = _resourceManager.GetString("SaveNameLabelKey", cultureInfo);
        InputFolderLabel.Text = _resourceManager.GetString("InputFolderLabelKey", cultureInfo);
        OutputFolderLabel.Text = _resourceManager.GetString("OutputFolderLabelKey", cultureInfo);
        SaveTypeLabel.Text = _resourceManager.GetString("SaveTypeLabelKey", cultureInfo);
        DeleteButton.Text = _resourceManager.GetString("DeleteButtonKey", cultureInfo);
        SaveButton.Text = _resourceManager.GetString("ValidateKey", cultureInfo);
        RunSaveButton.Text = _resourceManager.GetString("RunSaveButtonKey", cultureInfo);
    }

    protected override void OnAppearing()
    {
        if (viewModel.Saves.Count == 0)
        {
            viewModel.GetSauvegardes();
        }
        //viewModel.GetSauvegardes();

        SavesCollection.ItemsSource = viewModel.Saves;

        DeleteButton.IsVisible = false;

        ResetInput();

        // Fait apparaitre la page principale
        base.OnAppearing();
    }

    private void ResetInput()
    {
        EntrySaveName.Text = string.Empty;
        EntrySaveInputFolder.Text = string.Empty;
        EntrySaveOutputFolder.Text = string.Empty;
        EntrySaveType.SelectedItem = SaveType.COMPLETE;
        EntrySaveType.SelectedIndex = 0;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsNew = false;
        FormulaireConfiguration.IsVisible = true;

        var cultureInfo = App.LanguageService.CurrentLanguage;

        TitleConfigurationLabel.Text = _resourceManager.GetString("TitleConfigurationLabelKey", cultureInfo);

        viewModel.SetSelectedSave((Save)e.CurrentSelection.FirstOrDefault());

        if (viewModel.SelectedSave == null)
        {
            await DisplayAlert("Attention Problème", "attention", "Ok");
        }
        else
        {
            EntrySaveName.Text = viewModel.SelectedSave.Name;
            EntrySaveInputFolder.Text = viewModel.SelectedSave.InputFolder;
            EntrySaveOutputFolder.Text = viewModel.SelectedSave.OutputFolder;
            EntrySaveType.SelectedItem = viewModel.SelectedSave.SaveType.ToString();

            DeleteButton.IsVisible = true;
        }
    }

    private void ReturnHome_Clicked(object sender, EventArgs e)
    {
        IsNew = false;
        DeleteButton.IsVisible = false;
        FormulaireConfiguration.IsVisible = false;

        ResetInput();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (IsNew)
        {
            viewModel.AddSave(EntrySaveName.Text, EntrySaveInputFolder.Text, EntrySaveOutputFolder.Text, (SaveType)Enum.Parse(typeof(SaveType), EntrySaveType.SelectedItem.ToString()));

            IsNew = false;
            DeleteButton.IsVisible = true;
            var cultureInfo = App.LanguageService.CurrentLanguage;

            TitleConfigurationLabel.Text = _resourceManager.GetString("TitleConfigurationLabelKey", cultureInfo);
        }
        else
        {
            viewModel.UpdateSave(EntrySaveName.Text, EntrySaveInputFolder.Text, EntrySaveOutputFolder.Text, (SaveType)Enum.Parse(typeof(SaveType), EntrySaveType.SelectedItem.ToString()));
        }
        viewModel.GetSauvegardes();
        SavesCollection.ItemsSource = viewModel.Saves;
    }

    private void AddSaveButton_Clicked(object sender, EventArgs e)
    {
        IsNew = true;
        FormulaireConfiguration.IsVisible = true;
        DeleteButton.IsVisible= false;
        var cultureInfo = App.LanguageService.CurrentLanguage;

        TitleConfigurationLabel.Text = _resourceManager.GetString("NewTitleConfigurationLabelKey", cultureInfo);

        ResetInput();
    }

    private async void ParametersButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ParametersPage), false);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        viewModel.RemoveSave();
        ResetInput();
        viewModel.GetSauvegardes();
        SavesCollection.ItemsSource = viewModel.Saves;
        FormulaireConfiguration.IsVisible = false;
    }

    private async void ButtonNavigation_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RunSavesPage), false);
    }

    private async void PickInputFolder(object sender, EventArgs e)
    {
        await PickFolder(true);
    }

    private async void PickOutputFolder(object sender, EventArgs e)
    {
        await PickFolder(false);
    }

    private async Task PickFolder(bool IsInput)
    {
        try
        {
            FolderPickerResult folder = await FolderPicker.PickAsync(default);

            if (!folder.IsSuccessful)
                return;

            if (IsInput)
            {
                EntrySaveInputFolder.Text = folder.Folder.Path;
            }
            else
            {
                EntrySaveOutputFolder.Text = folder.Folder.Path;
            }
        }
        catch (Exception excp)
        {
            Debug.WriteLine(excp.Message);
        }
    }

    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        Save save = (Save)((Image)sender).BindingContext;

        viewModel.TogglePauseSave(save);

        // TODO: Vérifier qu'il n'y ait pas eu d'erreur lors de la mise en pause
        //if (save.State == SaveState.PAUSED)
        //{
        //    ((Image)sender).Source = "play.png";
        //}
        //else
        //{
        //    ((Image)sender).Source = "pause.png";
        //}
    }

    private void StopButton_Clicked(object sender, EventArgs e)
    {
        Save save = (Save)((Image)sender).BindingContext;

        viewModel.StopSave(save);
    }
}
