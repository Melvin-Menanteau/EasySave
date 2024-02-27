using System.Globalization;

namespace EasySaveUI
{
    public partial class App : Application
    {
        public static SharedLocalizer LanguageService { get; private set; }

        public App()
        {
            InitializeComponent();
            LanguageService = SharedLocalizer.GetInstance(new CultureInfo("en"));
            MainPage = new AppShell();
        }
    }
}
