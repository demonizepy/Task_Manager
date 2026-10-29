using System.Reflection;
using System.Windows;
using Task_Manager.Services;

namespace Task_Manager.Windows;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
        txtVersion.Text = LanguageManager.Format("About_Version", version?.ToString(2) ?? "");
    }
}
