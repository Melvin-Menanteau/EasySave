﻿using Microsoft.Maui.Controls;
using System.Resources;

namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{
    ParametersPageViewModel viewModel;
    string choice = "";

    private ResourceManager _resourceManager;
    public ParametersPage(ParametersPageViewModel viewModel)
    {
        InitializeComponent();

        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(LanguageService).Assembly);

        this.viewModel = viewModel;
        titlePage.Text = "Général";
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
                // Paramétrer les logiciel métier
                // Taille des fichiers
                // Fichiers prioritaires
                ParametersView.Content = new GeneralSettingsView();
                break;
            case "Chiffrement":
            case "Encryption":
                // Types de fichiers devant être chiffrés
                // Choix de la clé de chiffrement
                ParametersView.Content = new ChiffrementSettingsView();
                break;
            case "Langues":
            case "Languages":
                // Choix de la langue
                ParametersView.Content = new LanguesSettingsView();
                break;
            case "Journaux d'activités":
            case "Activity logs":
                // Affichage des journaux de logs
                ParametersView.Content = new JournauxSettingsView();
                break;
            case "Thèmes":
            case "Themes":
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

    private async void OnImageTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../", false);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        MessagingCenter.Unsubscribe<LanguesSettingsView>(this, "LanguageChanged");
    }
}