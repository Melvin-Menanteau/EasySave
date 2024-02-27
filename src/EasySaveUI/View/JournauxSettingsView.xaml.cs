using System.Resources;

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
    }
}