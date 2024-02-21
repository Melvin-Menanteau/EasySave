namespace EasySaveUI.View;

public partial class MainPage : ContentPage
{
    MainPageViewModel viewModel;

    public bool IsNew = false;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        viewModel.GetSauvegardes();

        SavesCollection.ItemsSource = viewModel.Saves;

        // Fait apparaitre la page principale
        base.OnAppearing();
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsNew = false;

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
            EntrySaveType.Text = viewModel.SelectedSave.SaveType.ToString();
        }
    }

    private void ReturnHome_Clicked(object sender, EventArgs e)
    {
        IsNew = false;

        EntrySaveName.Text = string.Empty;
        EntrySaveInputFolder.Text = string.Empty;
        EntrySaveOutputFolder.Text = string.Empty;
        EntrySaveType.Text = string.Empty;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (IsNew)
        {
            await DisplayAlert("Sauvegarde créée", "Nom : " + EntrySaveName, "Ok");
            IsNew = false;
        }
        else
        {
            await DisplayAlert("Sauvegarde Modifié", "Nom : " + EntrySaveName, "Ok");
        }
    }

    private void AddSaveButton_Clicked(object sender, EventArgs e)
    {
        IsNew = true;

        EntrySaveName.Text = string.Empty;
        EntrySaveInputFolder.Text = string.Empty;
        EntrySaveOutputFolder.Text = string.Empty;
        EntrySaveType.Text = string.Empty;
    }

    private void ParametersButton_Clicked(object sender, EventArgs e)
    {

    }
}
