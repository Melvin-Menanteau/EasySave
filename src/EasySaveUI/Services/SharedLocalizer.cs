using System.Globalization;
using System.Resources;

public class LanguageService
{
    private static LanguageService _instance;
    private CultureInfo _currentLanguage;
    private ResourceManager _resourceManager;

    public CultureInfo CurrentLanguage => _currentLanguage;

    private LanguageService(CultureInfo initialLanguage)
    {
        _currentLanguage = initialLanguage;
        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(LanguageService).Assembly);
    }

    public static LanguageService GetInstance(CultureInfo initialLanguage)
    {
        if (_instance == null)
        {
            _instance = new LanguageService(initialLanguage);
        }
        return _instance;
    }

    public void SetLanguage(CultureInfo culture)
    {
        _currentLanguage = culture;
    }

    public string GetString(string key)
    {
        return _resourceManager.GetString(key, _currentLanguage);
    }
}
