namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{
	public ParametersPage()
	{
		InitializeComponent();
	}

    private void OnImageTapped(object sender, EventArgs e)
    {
        DisplayAlert("Retour", "Vous allez retourner sur la page principal", "OK");
    }

    private void OnParameterClicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            // Parcourir tous les boutons dans le FlexLayout
            foreach (var childView in ParametersFlexLayout.Children)
            {
                if (childView is StackLayout stackLayout)
                {
                    foreach (var innerChild in stackLayout.Children)
                    {
                        if (innerChild is Button button)
                        {
                            // Réinitialiser le style des boutons autres que celui cliqué
                            if (button != clickedButton)
                            {
                                button.FontAttributes = FontAttributes.None;
                            }
                        }
                    }
                }
            }

            // Appliquer le style au bouton cliqué
            clickedButton.FontAttributes = FontAttributes.Bold;

            // Afficher le carré bleu uniquement pour le bouton cliqué
            foreach (var childView in ParametersFlexLayout.Children)
            {
                if (childView is StackLayout stackLayout && stackLayout.Children.Count > 0 && stackLayout.Children[0] is BoxView boxView)
                {
                    boxView.IsVisible = stackLayout.Children[1] == clickedButton;
                }
            }
        }
    }

}