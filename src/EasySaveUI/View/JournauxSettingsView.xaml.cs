namespace EasySaveUI.View;

public partial class JournauxSettingsView : ContentView
{
    ParametersPageViewModel viewModel;
    public JournauxSettingsView(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }
}