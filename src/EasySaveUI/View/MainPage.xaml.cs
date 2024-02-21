namespace EasySaveUI.View;

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
        }
    }

    private void ReturnHome_Clicked(object sender, EventArgs e)
    {
        IsNew = false;

        ResetInput();
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (IsNew)
        {
            viewModel.AddSave(EntrySaveName.Text, EntrySaveInputFolder.Text, EntrySaveOutputFolder.Text, (SaveType)Enum.Parse(typeof(SaveType), EntrySaveType.SelectedItem.ToString()));

            IsNew = false;
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

        TitleConfiguration.Text = "Nouvelle Configuration";

        ResetInput();
    }

    private void ParametersButton_Clicked(object sender, EventArgs e)
    {

    }
}
