﻿using Microsoft.Maui.Controls;

namespace EasySaveUI.View;

public partial class ParametersPage : ContentPage
{
    ParametersPageViewModel viewModel;
    string choice = "";
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
                ParametersView.Content = new GeneralSettingsView();
                break;
            case "Chiffrement":
                ParametersView.Content = new ChiffrementSettingsView();
                break;
            case "Langues":
                ParametersView.Content = new LanguesSettingsView();
                break;
            case "Thèmes":
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
        await DisplayAlert("Retour", "Vous allez retourner sur la page principal", "OK");
        await Shell.Current.GoToAsync(nameof(MainPage), false);
    }
}