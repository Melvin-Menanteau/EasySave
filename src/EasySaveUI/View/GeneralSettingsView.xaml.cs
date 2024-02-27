namespace EasySaveUI.View;

public partial class GeneralSettingsView : ContentView
{
    ParametersPageViewModel viewModel;
    private static Parameters? _instance;

    public List<string> EncryptionExstensionsList = [];
    public List<string> PriorityExtensionsList = [];
    public List<string> BusinessApplicationsList = [];
    public string EncryptionKey;
    public int MaxFileSize;
    public GeneralSettingsView(ParametersPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;

        OnPageAppearing();
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
        MaxFileSize = int.Parse(TailleFichiersEntry.Text);
        if (MaxFileSize == null)
        {
            MaxFileSize = 0;
        }
        PriorityExtensionsList = [.. editorPriority.Text.Split(";")];
        PriorityExtensionsList = PriorityExtensionsList.Select(str => str.Replace(" ", string.Empty).Replace(".", string.Empty)).ToList();
        PriorityExtensionsList = PriorityExtensionsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
        BusinessApplicationsList = editorMetiers.Text.Split(";").ToList();
        BusinessApplicationsList = BusinessApplicationsList.Select(str => str.Replace(" ", string.Empty)).ToList();
        BusinessApplicationsList = BusinessApplicationsList.Where(str => !string.IsNullOrEmpty(str)).ToList();

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