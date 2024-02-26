using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;

namespace EasySaveUI
{
    public static class MauiProgram
    {        

        public static MauiApp CreateMauiApp()
        {
            new Thread(new ThreadStart(() => BusinessObserver.Observer("CalculatorApp"))).Start();

            var processes = Process.GetProcessesByName("EasySaveUI");
            if (processes.Length > 1)
            {
                Process.GetCurrentProcess().Kill();
            }

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<ParametersPageViewModel>();
            builder.Services.AddTransient<ParametersPage>();

            builder.Services.AddTransient<RunSavesPageViewModel>();
            builder.Services.AddTransient<RunSavesPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
