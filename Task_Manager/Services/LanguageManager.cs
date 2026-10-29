using System.Windows;
using Task_Manager.Settings;

namespace Task_Manager.Services;

public static class LanguageManager
{
    public static AppLanguage Current { get; private set; } = AppLanguage.Russian;

    public static void Apply(AppLanguage language)
    {
        string fileName = language == AppLanguage.English ? "en" : "ru";
        ResourceSwitcher.Replace("Languages", fileName);
        Current = language;
    }

    public static string Get(string key)
    {
        return Application.Current.TryFindResource(key) as string ?? key;
    }

    public static string Format(string key, params object[] args)
    {
        return string.Format(Get(key), args);
    }
}
