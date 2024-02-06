using System;
using System.Globalization;
using System.Resources;

namespace EasySave
{
    public class SharedLocalizer
    {
        private static SharedLocalizer _instance;
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCulture;

        /// <summary>
        /// Rendre le constructeur prive afin d'empecher l'instanciation de la classe
        /// </summary>
        private SharedLocalizer()
        {
            _resourceManager = new ResourceManager("EasySave.Resources.Strings", typeof(Program).Assembly);
            _currentCulture = CultureInfo.CurrentCulture;
        }

        public static SharedLocalizer GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SharedLocalizer();
            }

            return _instance;
        }

        public void SetCulture(CultureInfo culture)
        {
            _currentCulture = culture;
        }

        public string GetLocalizedString(string key)
        {
            return _resourceManager.GetString(key, _currentCulture);
        }
    }
}
