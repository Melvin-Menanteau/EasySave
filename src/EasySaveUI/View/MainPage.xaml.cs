namespace EasySaveUI.View;

public partial class MainPage : ContentPage
{
    MainPageViewModel viewModel;

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
            EntrySaveIsSelected.Text = viewModel.SelectedSave.IsSelected.ToString();
        }
    }
}
