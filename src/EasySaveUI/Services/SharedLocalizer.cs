using System.Globalization;
using System.Resources;

public class SharedLocalizer
{
    private static SharedLocalizer _instance;
    private CultureInfo _currentLanguage;
    private ResourceManager _resourceManager;

    public CultureInfo CurrentLanguage => _currentLanguage;

    private SharedLocalizer(CultureInfo initialLanguage)
    {
        _currentLanguage = initialLanguage;
        _resourceManager = new ResourceManager("EasySaveUI.Resources.Langues.Langues", typeof(SharedLocalizer).Assembly);
    }

    public static SharedLocalizer GetInstance(CultureInfo initialLanguage)
    {
        if (_instance == null)
        {
            _instance = new SharedLocalizer(initialLanguage);
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
