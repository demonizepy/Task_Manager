using System.Windows;
using Task_Manager.Settings;

namespace Task_Manager.Windows;

public partial class SettingsWindow : Window
{
    public SettingsWindow(AppSettings current)
    {
        InitializeComponent();

        rbLight.IsChecked = current.Theme == AppTheme.Light;
        rbDark.IsChecked = current.Theme == AppTheme.Dark;
        rbRussian.IsChecked = current.Language == AppLanguage.Russian;
        rbEnglish.IsChecked = current.Language == AppLanguage.English;
    }

    public AppTheme SelectedTheme { get; private set; }
    public AppLanguage SelectedLanguage { get; private set; }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        SelectedTheme = rbDark.IsChecked == true ? AppTheme.Dark : AppTheme.Light;
        SelectedLanguage = rbEnglish.IsChecked == true ? AppLanguage.English : AppLanguage.Russian;
        DialogResult = true;
    }
}
