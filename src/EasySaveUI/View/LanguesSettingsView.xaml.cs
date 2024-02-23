using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace EasySaveUI.View
{
    public partial class LanguesSettingsView : ContentView
    {
        // Définir la liste des langues
        public List<string> Languages { get; set; }

        public LanguesSettingsView()
        {
            InitializeComponent();

            // Initialiser la liste des langues
            Languages = new List<string>
            {
                "Anglais",
                "Français"
            };

            // Définir le contexte de liaison pour accéder aux membres du code-behind depuis le XAML
            this.BindingContext = this;
        }

        // Gérer la sélection de langue
        private void OnLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            string selectedLanguage = picker.SelectedItem.ToString();

            // Vous pouvez ajouter votre logique de gestion de langue ici
            // Par exemple, enregistrer la langue sélectionnée dans les préférences ou configurer la langue de l'application en conséquence.
        }
    }
}
