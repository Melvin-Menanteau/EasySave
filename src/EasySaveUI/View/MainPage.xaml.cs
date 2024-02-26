namespace EasySaveUI.View;

using CommunityToolkit.Maui.Storage;
using System.Diagnostics;

public partial class MainPage : ContentPage
{
    MainPageViewModel viewModel;

    public bool IsNew = false;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;

        foreach (var saveType in Enum.GetValues(typeof(SaveType)))
        {
            EntrySaveType.Items.Add(saveType.ToString());
        }
    }

    protected override void OnAppearing()
    {
        viewModel.GetSauvegardes();

        SavesCollection.ItemsSource = viewModel.Saves;

        DeleteButton.IsVisible = false;

        // Fait apparaitre la page principale
        base.OnAppearing();
    }

    private void ResetInput()
    {
        EntrySaveName.Text = string.Empty;
        EntrySaveInputFolder.Text = string.Empty;
        EntrySaveOutputFolder.Text = string.Empty;
        EntrySaveType.SelectedItem = null;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsNew = false;
        TitleConfiguration.Text = "Configuration";

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

        ResetInput();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (IsNew)
        {
            viewModel.AddSave(EntrySaveName.Text, EntrySaveInputFolder.Text, EntrySaveOutputFolder.Text, (SaveType)Enum.Parse(typeof(SaveType), EntrySaveType.SelectedItem.ToString()));

            IsNew = false;
            DeleteButton.IsVisible = true;
            TitleConfiguration.Text = "Configuration";
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
        DeleteButton.IsVisible= false;

        TitleConfiguration.Text = "Nouvelle Configuration";

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

    }

    private async void ButtonNavigation_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RunSavesPage), false);
    }

    private async void PickInputFolder(object sender, EventArgs e)
    {
        PickFolder(true);
    }

    private async void PickOutputFolder(object sender, EventArgs e)
    {
        PickFolder(false);
    }

    private async void PickFolder(bool IsInput)
    {
        try
        {
            FolderPickerResult folder = await FolderPicker.PickAsync(default);

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
}
