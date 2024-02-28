using Microsoft.Maui.Controls;
using System.Resources;

namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{

    ParametersPageViewModel viewModel;
    string choice = "";
    private ResourceManager _resourceManager;
    private bool returnPressed = false;

    public ParametersPage(ParametersPageViewModel viewModel)
    {
        InitializeComponent();

        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);

        this.viewModel = viewModel;
        GeneralBoxView.IsVisible = true;
        GeneralButton.FontAttributes = FontAttributes.Bold;

        UpdateParametersView(choice);
        MessagingCenter.Subscribe<LanguesSettingsView>(this, "LanguageChanged", (sender) =>
        {
            LoadLocalizedTexts();
        });
        LoadLocalizedTexts();
    }

    private void LoadLocalizedTexts()
    {
        var cultureInfo = App.LanguageService.CurrentLanguage;
        titlePage.Text = _resourceManager.GetString("GeneralKey", cultureInfo);
        GeneralButton.Text = _resourceManager.GetString("GeneralKey", cultureInfo);
        ChiffrementButton.Text = _resourceManager.GetString("ChiffrementKey", cultureInfo);
        LanguesButton.Text = _resourceManager.GetString("LanguesKey", cultureInfo);
        JournauxButton.Text = _resourceManager.GetString("JournauxKey", cultureInfo);
        ThemesButton.Text = _resourceManager.GetString("ThemesKey", cultureInfo);
    }
    private void UpdateParametersView(string choice)
    {
        switch (choice)
        {
            case "Général":
            case "General":
            default:
                titlePage.Text = _resourceManager.GetString("GeneralKey", App.LanguageService.CurrentLanguage);
                ParametersView.Content = new GeneralSettingsView(viewModel);
                break;
            case "Chiffrement":
            case "Encryption":
                titlePage.Text = _resourceManager.GetString("ChiffrementKey", App.LanguageService.CurrentLanguage);
                ParametersView.Content = new ChiffrementSettingsView(viewModel);
                break;
            case "Langues":
            case "Languages":
                titlePage.Text = _resourceManager.GetString("LanguesKey", App.LanguageService.CurrentLanguage);
                ParametersView.Content = new LanguesSettingsView();
                break;
            case "Journaux d'activités":
            case "Activity Logs":
                titlePage.Text = _resourceManager.GetString("JournauxKey", App.LanguageService.CurrentLanguage);
                ParametersView.Content = new JournauxSettingsView(viewModel);
                break;
            case "Thèmes":
            case "Themes":
                titlePage.Text = _resourceManager.GetString("ThemesKey", App.LanguageService.CurrentLanguage);
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        MessagingCenter.Unsubscribe<LanguesSettingsView>(this, "LanguageChanged");
    }
}