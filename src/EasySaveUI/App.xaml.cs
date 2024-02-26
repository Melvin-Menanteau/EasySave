using System.Globalization;

namespace EasySaveUI
{
    public partial class App : Application
    {
        public static LanguageService LanguageService { get; private set; }

        public App()
        {
            InitializeComponent();
            LanguageService = LanguageService.GetInstance(new CultureInfo("en"));
            MainPage = new AppShell();
        }
    }
}
