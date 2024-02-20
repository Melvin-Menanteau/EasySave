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
        Save save = (Save)e.CurrentSelection.FirstOrDefault();

        if (save == null)
        {
            await DisplayAlert("Attention Problème", "attention", "Ok");
        }
        else
        {
            EntrySaveName.Text = save.Name;
            EntrySaveInputFolder.Text = save.InputFolder;
            EntrySaveOutputFolder.Text = save.OutputFolder;
            EntrySaveType.Text = save.SaveType.ToString();
        }
    }
}
