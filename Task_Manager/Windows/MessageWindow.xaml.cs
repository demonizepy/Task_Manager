using System.Windows;

namespace Task_Manager.Windows;

public enum MessageKind
{
    Warning,
    Error,
    Question
}

public partial class MessageWindow : Window
{
    public MessageWindow(string text, MessageKind kind)
    {
        InitializeComponent();
        txtMessage.Text = text;

        switch (kind)
        {
            case MessageKind.Warning:
                txtIcon.Text = "\uE7BA";
                txtIcon.SetResourceReference(ForegroundProperty, "WarningBrush");
                break;
            case MessageKind.Error:
                txtIcon.Text = "\uE783";
                txtIcon.SetResourceReference(ForegroundProperty, "ErrorBrush");
                break;
            case MessageKind.Question:
                txtIcon.Text = "\uE9CE";
                txtIcon.SetResourceReference(ForegroundProperty, "AccentBrush");
                break;
        }

        bool question = kind == MessageKind.Question;
        btnYes.Visibility = question ? Visibility.Visible : Visibility.Collapsed;
        btnNo.Visibility = question ? Visibility.Visible : Visibility.Collapsed;
        btnOk.Visibility = question ? Visibility.Collapsed : Visibility.Visible;

        // Enter и Esc должны срабатывать только на видимых кнопках
        btnYes.IsDefault = question;
        btnNo.IsCancel = question;
        btnOk.IsDefault = !question;
        btnOk.IsCancel = !question;
    }

    private void btnYes_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
