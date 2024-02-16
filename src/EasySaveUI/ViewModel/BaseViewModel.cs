namespace EasySave.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    // Variable définnisant le titre de la page dans laquelle on se trouve
    [ObservableProperty]
    string title;

    public BaseViewModel()
    {

    }
}
