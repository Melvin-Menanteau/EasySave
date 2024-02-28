using System.Resources;
using System.Text;

namespace EasySaveUI.View;

public partial class JournauxSettingsView : ContentView
{
    ParametersPageViewModel viewModel;
    private ResourceManager _resourceManager;
    public JournauxSettingsView(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;

        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);
        MessagingCenter.Subscribe<LanguesSettingsView>(this, "LanguageChanged", (sender) =>
        {
            LoadLocalizedTexts();
        });
        LoadLocalizedTexts();
    }

    private void LoadLocalizedTexts()
    {
        var cultureInfo = App.LanguageService.CurrentLanguage;
        DailyXMLLabel.Text = _resourceManager.GetString("DailyXMLLabelKey", cultureInfo);
        DailyJSONLabel.Text = _resourceManager.GetString("DailyJSONLabelKey", cultureInfo);
        StateLogJSON.Text = _resourceManager.GetString("StateLogJSONKey", cultureInfo);
        // Concaténer les entrées XML en une seule chaîne

        StringBuilder xmlEntriesBuilder = new StringBuilder();
        StringBuilder jsonEntriesBuilder = new StringBuilder();
        StringBuilder statusEntriesBuilder = new StringBuilder();
        foreach (string entry in viewModel.LoadXmlLogEntries())
        {
            xmlEntriesBuilder.AppendLine(entry);
        }
        XmlLogEntries.Text = xmlEntriesBuilder.ToString();
        foreach (string entry in viewModel.LoadJsonLogEntries())
        {
            jsonEntriesBuilder.AppendLine(entry);
        }
        JsonLogEntries.Text = jsonEntriesBuilder.ToString();
        foreach (string entry in viewModel.LoadStatusLogEntries())
        {
            statusEntriesBuilder.AppendLine(entry);
        }
        StatusLogEntries.Text = statusEntriesBuilder.ToString();
    }
}