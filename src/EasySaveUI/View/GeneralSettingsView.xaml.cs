namespace EasySaveUI.View;

public partial class GeneralSettingsView : ContentView
{
    public GeneralSettingsView()
    {
        InitializeComponent();
        Content = new StackLayout
        {
            Children =
                {
                    new Label
                    {
                        Text = "Bienvenue dans l'application EasySaveUI !",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                }
        };
    }
}