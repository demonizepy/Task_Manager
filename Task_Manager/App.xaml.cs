using System.Windows;
using Task_Manager.Core.Services;
using Task_Manager.Services;
using Task_Manager.Settings;
using Task_Manager.Windows;

namespace Task_Manager;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var settingsStorage = new SettingsStorage(Path.Combine(AppContext.BaseDirectory, "settings.json"));
        AppSettings settings = settingsStorage.Load();

        ThemeManager.Init();
        ThemeManager.Apply(settings.Theme);
        LanguageManager.Apply(settings.Language);

        // здесь модуль Task_Manager.Core подключается к окнам
        string path = Path.Combine(AppContext.BaseDirectory, "tasks.txt");
        var manager = new TaskManager(new TaskStorage(path));

        var window = new MainWindow(manager, settingsStorage, settings);
        MainWindow = window;
        window.Show();
    }
}
