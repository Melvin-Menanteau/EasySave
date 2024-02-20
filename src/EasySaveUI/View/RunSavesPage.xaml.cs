namespace EasySaveUI.View;

public partial class RunSavesPage : ContentPage
{
	public RunSavesPage()
	{
		InitializeComponent();
	}

    
}

public class VotreViewModel : BindableObject
{
    private object _emptyView = "Aucune Sauvegarde";

    public object EmptyView
    {
        get { return _emptyView; }
        set
        {
            if (_emptyView != value)
            {
                _emptyView = value;
                OnPropertyChanged();
            }
        }
    }
}