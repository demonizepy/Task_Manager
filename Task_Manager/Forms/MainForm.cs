using Task_Manager.Models;
using Task_Manager.Services;

namespace Task_Manager.Forms;

public partial class MainForm : Form
{
    private const string AppTitle = "Менеджер задач";

    private readonly TaskManager manager;

    public MainForm()
    {
        InitializeComponent();

        string path = Path.Combine(AppContext.BaseDirectory, "tasks.txt");
        manager = new TaskManager(new TaskStorage(path));
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        manager.Load();
        RefreshList();
    }

    private void RefreshList()
    {
        listTasks.BeginUpdate();
        listTasks.Items.Clear();

        foreach (TaskItem task in manager.GetAll())
        {
            var item = new ListViewItem(task.Id.ToString());
            item.SubItems.Add(task.Description);
            item.Tag = task.Id;
            listTasks.Items.Add(item);
        }

        listTasks.EndUpdate();
        lblCount.Text = "Задач: " + manager.Count;
    }

    private void ShowWarning(string text)
    {
        MessageBox.Show(text, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string description = txtDescription.Text.Trim();

        if (description == "")
        {
            ShowWarning("Введите описание задачи.");
            txtDescription.Focus();
            return;
        }

        manager.Add((int)numId.Value, description);
        manager.Save();

        txtDescription.Clear();
        txtDescription.Focus();
        RefreshList();
    }

    private void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        if (listTasks.SelectedItems.Count == 0)
        {
            ShowWarning("Сначала выберите задачи в списке.");
            return;
        }

        foreach (ListViewItem item in listTasks.SelectedItems)
        {
            manager.Remove((int)item.Tag!);
        }

        manager.Save();
        RefreshList();
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        if (manager.Count == 0)
        {
            ShowWarning("Список и так пустой.");
            return;
        }

        DialogResult answer = MessageBox.Show("Точно очистить весь список?", AppTitle,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (answer != DialogResult.Yes)
            return;

        manager.Clear();
        manager.Save();
        RefreshList();
    }
}
