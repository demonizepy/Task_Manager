using System.Windows;

namespace Task_Manager.Windows;

public static class Dialogs
{
    public static void Warning(Window owner, string text)
    {
        Show(owner, text, MessageKind.Warning);
    }

    public static void Error(Window owner, string text)
    {
        Show(owner, text, MessageKind.Error);
    }

    public static bool Confirm(Window owner, string text)
    {
        return Show(owner, text, MessageKind.Question);
    }

    // обычный MessageBox не умеет в тёмную тему и пишет "Да/Нет" на языке Windows,
    // поэтому сообщения показываются своим окном
    private static bool Show(Window owner, string text, MessageKind kind)
    {
        var window = new MessageWindow(text, kind) { Owner = owner };
        return window.ShowDialog() == true;
    }
}
