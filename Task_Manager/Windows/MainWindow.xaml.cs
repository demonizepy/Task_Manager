using System.Windows;
using System.Windows.Input;
using Task_Manager.Core.Models;
using Task_Manager.Core.Services;
using Task_Manager.Services;
using Task_Manager.Settings;

namespace Task_Manager.Windows;

public partial class MainWindow : Window
{
    private readonly TaskManager manager;
    private readonly SettingsStorage settingsStorage;
    private readonly AppSettings settings;

    public MainWindow(TaskManager manager, SettingsStorage settingsStorage, AppSettings settings)
    {
        InitializeComponent();
        this.manager = manager;
        this.settingsStorage = settingsStorage;
        this.settings = settings;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            manager.Load();
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            Dialogs.Error(this, LanguageManager.Get("Msg_LoadError") + Environment.NewLine + ex.Message);
        }

        RefreshList();
    }

    private void RefreshList()
    {
        gridTasks.ItemsSource = manager.GetAll();
        txtCount.Text = LanguageManager.Format("Status_Count", manager.Count);
    }

    private void SaveTasks()
    {
        try
        {
            manager.Save();
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            Dialogs.Error(this, LanguageManager.Get("Msg_SaveError") + Environment.NewLine + ex.Message);
        }
    }

    private void DeleteSelected()
    {
        List<int> ids = gridTasks.SelectedItems.Cast<TaskItem>().Select(t => t.Id).ToList();

        if (ids.Count == 0)
        {
            Dialogs.Warning(this, LanguageManager.Get("Msg_SelectTasks"));
            return;
        }

        foreach (int id in ids)
        {
            manager.Remove(id);
        }

        SaveTasks();
        RefreshList();
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        var window = new AddTaskWindow(manager.GetNextId(), manager.Contains) { Owner = this };

        if (window.ShowDialog() != true)
            return;

        manager.Add(window.TaskId, window.TaskDescription);
        SaveTasks();
        RefreshList();
    }

    private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
        DeleteSelected();
    }

    private void btnDeleteIds_Click(object sender, RoutedEventArgs e)
    {
        var window = new DeleteByIdWindow { Owner = this };

        if (window.ShowDialog() != true)
            return;

        RemoveResult result = manager.RemoveByIds(window.IdsText);

        if (result.IsEmpty)
        {
            Dialogs.Warning(this, LanguageManager.Get("DeleteIds_Hint"));
            return;
        }

        if (result.Removed.Count > 0)
        {
            SaveTasks();
            RefreshList();
        }

        // если всё удалилось без проблем, сообщение не нужно
        if (result.NotFound.Count == 0 && result.Invalid.Count == 0)
            return;

        var lines = new List<string>();

        if (result.Removed.Count > 0)
            lines.Add(LanguageManager.Format("Msg_Removed", string.Join(", ", result.Removed)));

        if (result.NotFound.Count > 0)
            lines.Add(LanguageManager.Format("Msg_NotFound", string.Join(", ", result.NotFound)));

        if (result.Invalid.Count > 0)
            lines.Add(LanguageManager.Format("Msg_Invalid", string.Join(", ", result.Invalid)));

        Dialogs.Warning(this, string.Join(Environment.NewLine, lines));
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
        if (manager.Count == 0)
        {
            Dialogs.Warning(this, LanguageManager.Get("Msg_ListEmpty"));
            return;
        }

        if (!Dialogs.Confirm(this, LanguageManager.Get("Msg_ConfirmClear")))
            return;

        manager.Clear();
        SaveTasks();
        RefreshList();
    }

    private void btnSettings_Click(object sender, RoutedEventArgs e)
    {
        var window = new SettingsWindow(settings) { Owner = this };

        if (window.ShowDialog() != true)
            return;

        settings.Theme = window.SelectedTheme;
        settings.Language = window.SelectedLanguage;

        ThemeManager.Apply(settings.Theme);
        LanguageManager.Apply(settings.Language);

        // строка состояния собирается в коде, её надо пересобрать на новом языке
        RefreshList();

        try
        {
            settingsStorage.Save(settings);
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            Dialogs.Error(this, LanguageManager.Get("Msg_SettingsError") + Environment.NewLine + ex.Message);
        }
    }

    private void btnAbout_Click(object sender, RoutedEventArgs e)
    {
        new AboutWindow { Owner = this }.ShowDialog();
    }

    private void gridTasks_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete && gridTasks.SelectedItems.Count > 0)
        {
            DeleteSelected();
            e.Handled = true;
        }
    }
}
