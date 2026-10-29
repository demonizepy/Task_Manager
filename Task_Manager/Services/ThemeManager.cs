using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Task_Manager.Settings;

namespace Task_Manager.Services;

public static class ThemeManager
{
    public static AppTheme Current { get; private set; } = AppTheme.Light;

    public static void Init()
    {
        // каждое новое окно при открытии получает заголовок в цвет темы
        EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.LoadedEvent,
            new RoutedEventHandler((sender, e) => UpdateTitleBar((Window)sender)));
    }

    public static void Apply(AppTheme theme)
    {
        ResourceSwitcher.Replace("Themes", theme.ToString());
        Current = theme;

        foreach (Window window in Application.Current.Windows)
        {
            UpdateTitleBar(window);
        }
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

    private static void UpdateTitleBar(Window window)
    {
        IntPtr handle = new WindowInteropHelper(window).Handle;

        if (handle == IntPtr.Zero)
            return;

        int dark = Current == AppTheme.Dark ? 1 : 0;

        // 20 работает на свежих сборках Windows 10, 19 - на сборках до 20H1
        if (DwmSetWindowAttribute(handle, 20, ref dark, sizeof(int)) != 0)
            DwmSetWindowAttribute(handle, 19, ref dark, sizeof(int));
    }
}
