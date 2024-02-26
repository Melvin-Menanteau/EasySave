namespace EasySaveUI.View;

public partial class RunSavesPage : ContentPage
{
    RunSavesPageViewModel viewModel;
    List<Save> SavesSelected = new List<Save>();
    private bool returnPressed = false;

    public RunSavesPage(RunSavesPageViewModel viewModel)
	{
		InitializeComponent();
        this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        viewModel.GetSauvegardes();

        SavesSelectedCollection.ItemsSource = viewModel.Saves;

        // Fait apparaitre la page principale
        base.OnAppearing();
    }

    private async void OnImageTapped(object sender, EventArgs e)
    {
        if (!returnPressed)
        {
            await Shell.Current.GoToAsync("../", false);
        }
        returnPressed = true;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;

        Save Save = viewModel.Saves.FirstOrDefault(x => x.Id == (checkBox.BindingContext as Save).Id);

        if (checkBox.IsChecked)
        {
            SavesSelected.Add(Save);
        }
        else
        {
            SavesSelected.Remove(Save);
        }
        EditorSaveEditor();
    }

    private void EditorSaveEditor()
    {
        SavesEditor.Text = string.Empty;
        foreach (var save in SavesSelected)
        {
            SavesEditor.Text += save.Id + "; ";
        }
    }

    private void RunSavesButton_Clicked(object sender, EventArgs e)
    {

    }
}