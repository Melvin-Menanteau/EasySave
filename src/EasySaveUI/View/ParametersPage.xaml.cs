using Microsoft.Maui.Controls;

namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{
    ParametersPageViewModel viewModel;
    public ParametersPage(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        titlePage.Text = "Général";
        GeneralBoxView.IsVisible = true;
        GeneralButton.FontAttributes = FontAttributes.Bold;

        Label label = new Label();
        label.Text = "Text";
        label.FontSize = 50;

        ReglagesFlexLayout.Children.Add(label);
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Retour", "Vous allez retourner sur la page principal", "OK");
        await Shell.Current.GoToAsync(nameof(MainPage), false);
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
            titlePage.Text = clickedButton.Text;
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

            switch (clickedButton.Text)
            {
                case "Général":
                    ReglagesFlexLayout.Children.Clear();

                    Label label = new Label();
                    label.Text = "Général";
                    label.FontSize = 50;

                    ReglagesFlexLayout.Children.Add(label);
                    this.InvalidateMeasure();

                    break;
                case "Chiffrement":
                    ReglagesFlexLayout.Children.Clear();

                    Label label1 = new Label();
                    label1.Text = "Chiffrement";
                    label1.FontSize = 50;

                    ReglagesFlexLayout.Children.Add(label1);
                    this.ForceLayout();
                    break;
                case "Langues":
                    ReglagesFlexLayout.Children.Clear();

                    Picker picker = new Picker
                    {
                        Title = "Sélectionner une langue",
                        Items = { "Anglais", "Français" }
                    };
                    ReglagesFlexLayout.Children.Add(picker);

                    Button button = new Button
                    {
                        Text = "Valider"
                    };
                    button.Clicked += (s, args) =>
                    {
                        // Code de validation ici
                    };
                    ReglagesFlexLayout.Children.Add(button);
                    this.ForceLayout();

                    break;
                case "Thèmes":
                    ReglagesFlexLayout.Children.Clear();

                    Label label3 = new Label();
                    label3.Text = "Thèmes";
                    label3.FontSize = 50;

                    ReglagesFlexLayout.Children.Add(label3);
                    this.ForceLayout();
                    break;
                default:
                    break;
            }
        }
    }

}