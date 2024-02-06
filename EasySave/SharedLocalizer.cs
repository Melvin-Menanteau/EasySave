using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace EasySave
{
    public class SharedLocalizer
    {
        private static SharedLocalizer _instance;
        private static readonly ResourceManager _resourceManager = new ResourceManager("EasySave.Resources.locale", typeof(Program).Assembly);
        private static CultureInfo _currentCulture;

        /// <summary>
        /// Rendre le constructeur prive afin d'empecher l'instanciation de la classe
        /// </summary>
        private SharedLocalizer()
        {
            SetCulture("fr-FR");
        }

        public static SharedLocalizer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SharedLocalizer();
            }

            return _instance;
        }

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
