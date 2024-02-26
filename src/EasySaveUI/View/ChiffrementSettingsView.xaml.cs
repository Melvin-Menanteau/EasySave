namespace EasySaveUI.View;

public partial class ChiffrementSettingsView : ContentView
{
    ParametersPageViewModel viewModel;

    public List<string> ExtensionsList = [];

	public ChiffrementSettingsView(ParametersPageViewModel viewModel)
	{
		InitializeComponent();
        this.viewModel = viewModel;

        OnPageAppearing();
	}

    public void OnPageAppearing()
    {
        ExtensionsList = viewModel.GetExtensionList();
        if (ExtensionsList.Count > 0 )
        {
            foreach ( var extension in ExtensionsList )
            {
                editor.Text += "."+extension + "; ";
            }
        }
    }


    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        ExtensionsList = [.. editor.Text.Split(";")];
        ExtensionsList = ExtensionsList.Select(str => str.Replace(" ", string.Empty).Replace(".", string.Empty)).ToList();
        ExtensionsList = ExtensionsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
    }

    private void SaveEditorButton_Clicked(object sender, EventArgs e)
    {
        viewModel.SaveExtension(ExtensionsList);
    }
}