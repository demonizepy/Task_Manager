using System.Windows;
using Task_Manager.Services;

namespace Task_Manager.Windows;

public partial class DeleteByIdWindow : Window
{
    public DeleteByIdWindow()
    {
        InitializeComponent();
    }

    public string IdsText { get; private set; } = "";

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        txtIds.Focus();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        if (txtIds.Text.Trim() == "")
        {
            Dialogs.Warning(this, LanguageManager.Get("Msg_EnterId"));
            txtIds.Focus();
            return;
        }

        IdsText = txtIds.Text;
        DialogResult = true;
    }
}
