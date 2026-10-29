namespace Task_Manager.Settings;

public enum AppTheme
{
    Light,
    Dark
}

public enum AppLanguage
{
    Russian,
    English
}

public class AppSettings
{
    public AppTheme Theme { get; set; } = AppTheme.Light;
    public AppLanguage Language { get; set; } = AppLanguage.Russian;
}
