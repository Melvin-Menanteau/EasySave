using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace EasySaveUI.View
{
    public partial class LanguesSettingsView : ContentView
    {
        // D�finir la liste des langues
        public List<string> Languages { get; set; }

        public LanguesSettingsView()
        {
            InitializeComponent();

            // Initialiser la liste des langues
            Languages = new List<string>
            {
                "Anglais",
                "Fran�ais"
            };

            // D�finir le contexte de liaison pour acc�der aux membres du code-behind depuis le XAML
            this.BindingContext = this;
        }

        // G�rer la s�lection de langue
        private void OnLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            string selectedLanguage = picker.SelectedItem.ToString();

            // Vous pouvez ajouter votre logique de gestion de langue ici
            // Par exemple, enregistrer la langue s�lectionn�e dans les pr�f�rences ou configurer la langue de l'application en cons�quence.
        }
    }
}
