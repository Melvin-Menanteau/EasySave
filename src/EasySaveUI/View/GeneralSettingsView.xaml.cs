using System.Resources;

namespace EasySaveUI.View;

public partial class GeneralSettingsView : ContentView
{
    ParametersPageViewModel viewModel;
    
    private static Parameters _instance;
    public List<string> EncryptionExstensionsList = [];
    private ResourceManager _resourceManager;
    public List<string> PriorityExtensionsList = [];
    public List<string> BusinessApplicationsList = [];
    public string EncryptionKey;
    public int MaxFileSize;
    public GeneralSettingsView(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;

        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);
        MessagingCenter.Subscribe<LanguesSettingsView>(this, "LanguageChanged", (sender) =>
        {
            LoadLocalizedTexts();
        });
        LoadLocalizedTexts();
        OnPageAppearing();
    }

    private void LoadLocalizedTexts()
    {
        var cultureInfo = App.LanguageService.CurrentLanguage;
        SaveButton.Text = _resourceManager.GetString("ValidateKey", cultureInfo);
        FileSizeLabel.Text = _resourceManager.GetString("FileSizeLabelKey", cultureInfo);
        PriorityExtensionsLabel.Text = _resourceManager.GetString("PriorityExtensionsLabelKey", cultureInfo);
        BusinessAppLabel.Text = _resourceManager.GetString("BusinessAppLabelKey", cultureInfo);
    }
    public void OnPageAppearing()
    {
        MaxFileSize = viewModel.GetMaxFileSize();
        PriorityExtensionsList = viewModel.GetPriorityExtensionList();
        BusinessApplicationsList = viewModel.GetBusinessApplicationsList();

        if (MaxFileSize > 0)
        {
            TailleFichiersEntry.Text = MaxFileSize.ToString();
        }
        if (PriorityExtensionsList.Count > 0)
        {
            foreach (var extension in PriorityExtensionsList)
            {
                editorPriority.Text += "." + extension + "; ";
            }
        }
        if (BusinessApplicationsList.Count > 0)
        {
            foreach (var application in BusinessApplicationsList)
            {
                editorMetiers.Text += application + "; ";
            }
        }
    }

    private void OnValiderClicked(object sender, EventArgs e)
    {
        if ((TailleFichiersEntry.Text == null) || (TailleFichiersEntry.Text == ""))
        {
            MaxFileSize = 0;
        }
        else
        {
            MaxFileSize = int.Parse(TailleFichiersEntry.Text);
        }
        if (editorPriority.Text != null)
        {
            PriorityExtensionsList = [.. editorPriority.Text.Split(";")];
            PriorityExtensionsList = PriorityExtensionsList.Select(str => str.Replace(" ", string.Empty).Replace(".", string.Empty)).ToList();
            PriorityExtensionsList = PriorityExtensionsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }
        if (editorMetiers.Text != null)
        {
            BusinessApplicationsList = editorMetiers.Text.Split(";").ToList();
            BusinessApplicationsList = BusinessApplicationsList.Select(str => str.Replace(" ", string.Empty)).ToList();
            BusinessApplicationsList = BusinessApplicationsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }

        viewModel.SaveBusinessApplications(BusinessApplicationsList);
        viewModel.SaveMaxFileSize(MaxFileSize);
        viewModel.SavePriorityExtension(PriorityExtensionsList);
    }

    private void OnEditorPriorityTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnEditorPriorityCompleted(object sender, EventArgs e)
    {

    }

    private void OnEditorMetiersTextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void OnEditorMetiersCompleted(object sender, EventArgs e)
    {

    }
}