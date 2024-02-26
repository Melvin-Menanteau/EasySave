using Microsoft.Maui.Controls;

namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{

    ParametersPageViewModel viewModel;
    string choice = "";
    private bool returnPressed = false;

    public ParametersPage(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        titlePage.Text = "Général";
        GeneralBoxView.IsVisible = true;
        GeneralButton.FontAttributes = FontAttributes.Bold;

        UpdateParametersView(choice);
    }

    private void UpdateParametersView(string choice)
    {
        switch (choice)
        {
            case "Général":
            default:
                // Paramétrer les logiciel métier
                // Taille des fichiers
                // Fichiers prioritaires
                ParametersView.Content = new GeneralSettingsView();
                break;
            case "Chiffrement":
                // Types de fichiers devant être chiffrés
                // Choix de la clé de chiffrement
                ParametersView.Content = new ChiffrementSettingsView();
                break;
            case "Langues":
                // Choix de la langue
                ParametersView.Content = new LanguesSettingsView();
                break;
            case "Journaux d'activités":
                // Affichage des journaux de logs
                ParametersView.Content = new JournauxSettingsView();
                break;
            case "Thèmes":
                // Choix du thèmes
                ParametersView.Content = new ThemesSettingsView();
                break;
        }
    }

    private void OnParameterClicked(object sender, EventArgs e)
    {
        if (sender is Button clickedButton)
        {
            foreach (var childView in ParametersFlexLayout.Children)
            {
                if (childView is StackLayout stackLayout)
                {
                    foreach (var innerChild in stackLayout.Children)
                    {
                        if (innerChild is Button button)
                        {
                            if (button != clickedButton)
                            {
                                button.FontAttributes = FontAttributes.None;
                            }
                        }
                    }
                }
            }
            titlePage.Text = clickedButton.Text;
            clickedButton.FontAttributes = FontAttributes.Bold;

            foreach (var childView in ParametersFlexLayout.Children)
            {
                if (childView is StackLayout stackLayout && stackLayout.Children.Count > 0 && stackLayout.Children[0] is BoxView boxView)
                {
                    boxView.IsVisible = stackLayout.Children[1] == clickedButton;
                }
            }
            choice = clickedButton.Text;
            UpdateParametersView(choice);
        }
    }

    private async void ReturnHome_Clicked(object sender, EventArgs e)
    {
        if (!returnPressed)
        {
            await Shell.Current.GoToAsync("../", false);
        }
        returnPressed = true;
    }
}