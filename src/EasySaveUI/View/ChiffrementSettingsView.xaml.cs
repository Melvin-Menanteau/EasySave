using System.Resources;

namespace EasySaveUI.View;

public partial class ChiffrementSettingsView : ContentView
{
    ParametersPageViewModel viewModel;
    private ResourceManager _resourceManager;
    public List<string> EncryptionExstensionsList = [];
    public string EncryptionKey;

	public ChiffrementSettingsView(ParametersPageViewModel viewModel)
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
        SaveKeyButton.Text = _resourceManager.GetString("ValidateKey", cultureInfo);
        SaveEditorButton.Text = _resourceManager.GetString("ValidateKey", cultureInfo);
        ListTypeLabel.Text = _resourceManager.GetString("ListTypeLabelKey", cultureInfo);
        KeyLabel.Text = _resourceManager.GetString("KeyLabelKey", cultureInfo);
    }

    public void OnPageAppearing()
    {
        EncryptionExstensionsList = viewModel.GetEncryptionExtensionList();
        EncryptionKey = viewModel.GetEncryptionKey();
        if (EncryptionExstensionsList.Count > 0 )
        {
            foreach ( var extension in EncryptionExstensionsList )
            {
                editor.Text += "."+extension + "; ";
            }
        }
        editorKey.Text = EncryptionKey;

    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        EncryptionKey = editorKey.Text;
    }

    private void SaveKeyButton_Clicked(object sender, EventArgs e)
    {
        viewModel.SaveEncryptionKey(EncryptionKey);
    }

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        EncryptionExstensionsList = [.. editor.Text.Split(";")];
        EncryptionExstensionsList = EncryptionExstensionsList.Select(str => str.Replace(" ", string.Empty).Replace(".", string.Empty)).ToList();
        EncryptionExstensionsList = EncryptionExstensionsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
    }

    private void SaveEditorButton_Clicked(object sender, EventArgs e)
    {
        viewModel.SaveEncryptionExtension(EncryptionExstensionsList);
    }
}