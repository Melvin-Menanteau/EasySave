using System.Diagnostics;

namespace EasySaveUI.View;

public partial class ChiffrementSettingsView : ContentView
{
    List<string> ExtensionsList = new List<string>();

	public ChiffrementSettingsView()
	{
		InitializeComponent();
	}

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        string EditorText = editor.Text;

        if (EditorText != null)
        {
            ExtensionsList = EditorText.Split(";").ToList();
            ExtensionsList = ExtensionsList.Select(str => str.Replace(" ", string.Empty).Replace(".", string.Empty)).ToList();
            ExtensionsList = ExtensionsList.Where(str => !string.IsNullOrEmpty(str)).ToList();
        }
        
    }

    private void OnEditorCompleted(object sender, EventArgs e)
    {

    }

    private void SaveEditorButton_Clicked(object sender, EventArgs e)
    {
        foreach(string Extension in ExtensionsList)
        {
            Debug.WriteLine(Extension);
        }
        
    }
}