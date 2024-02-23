using EasySaveUI.View;

namespace EasySaveUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Routing.RegisterRoute(nameof(ParametersPage), typeof(ParametersPage));
            
            Routing.RegisterRoute(nameof(RunSavesPage), typeof(RunSavesPage));
        }
    }
}
