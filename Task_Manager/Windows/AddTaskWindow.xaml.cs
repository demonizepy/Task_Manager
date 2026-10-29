using System.Windows;
using Task_Manager.Services;

namespace Task_Manager.Windows;

public partial class AddTaskWindow : Window
{
    private readonly Func<int, bool> idExists;

    public AddTaskWindow(int suggestedId, Func<int, bool> idExists)
    {
        InitializeComponent();
        this.idExists = idExists;
        txtId.Text = suggestedId.ToString();
    }

    public int TaskId { get; private set; }
    public string TaskDescription { get; private set; } = "";

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        txtDescription.Focus();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(txtId.Text.Trim(), out int id) || id <= 0)
        {
            Dialogs.Warning(this, LanguageManager.Get("Msg_BadId"));
            txtId.Focus();
            txtId.SelectAll();
            return;
        }

        string description = txtDescription.Text.Trim();

        if (description == "")
        {
            Dialogs.Warning(this, LanguageManager.Get("Msg_EmptyDescription"));
            txtDescription.Focus();
            return;
        }

        if (idExists(id) && !Dialogs.Confirm(this, LanguageManager.Format("Msg_ReplaceTask", id)))
            return;

        TaskId = id;
        TaskDescription = description;
        DialogResult = true;
    }
}
