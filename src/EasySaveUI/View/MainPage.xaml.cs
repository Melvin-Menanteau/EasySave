
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

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
    }
}
