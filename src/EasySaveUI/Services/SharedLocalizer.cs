using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace EasySaveUI.Services
{
    public static class SharedLocalizer
    {
        private static readonly ResourceManager _resourceManager = new ResourceManager("EasySave.Resources.locale", typeof(MauiProgram).Assembly);

        public static void SetCulture(string culture)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }

        public static string GetLocalizedString(string key)
        {
            return _resourceManager.GetString(key);
        }
    }
}
