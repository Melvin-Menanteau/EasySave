using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Globalization;
using EasySaveUI.Services;
using System.Resources;

namespace EasySaveUI.View
{
    public partial class LanguesSettingsView : ContentView
    {
        private ResourceManager _resourceManager;

        public LanguesSettingsView()
        {
            InitializeComponent();

            // Initialize the ResourceManager with your resources
            _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);

            // Get the current culture info from the language service
            var currentLanguage = App.LanguageService.CurrentLanguage;

            // Check the current language and set the picker accordingly
            if (currentLanguage.Name == "en")
            {
                LanguagePicker.SelectedIndex = 0; // English
            }
            else if (currentLanguage.Name == "fr")
            {
                LanguagePicker.SelectedIndex = 1; // French
            }

            // Load the localized texts
            LoadLocalizedTexts();
        }

        private void LoadLocalizedTexts()
        {
            // Get the current culture info from the language service
            var cultureInfo = App.LanguageService.CurrentLanguage;

            // Load the localized texts using the ResourceManager
            SelectLanguageLabel.Text = _resourceManager.GetString("SelectLanguageKey", cultureInfo);
            ValidateButton.Text = _resourceManager.GetString("ValidateKey", cultureInfo);
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            var selectedLanguageIndex = LanguagePicker.SelectedIndex;

            var languageService = SharedLocalizer.GetInstance(new CultureInfo("en")); // Use the initial language

            if (selectedLanguageIndex == 0) // English
            {
                languageService.SetLanguage(new CultureInfo("en"));
            }
            else if (selectedLanguageIndex == 1) // French
            {
                languageService.SetLanguage(new CultureInfo("fr"));
            }
            MessagingCenter.Send(this, "LanguageChanged");
            LoadLocalizedTexts();
        }

    }
}
