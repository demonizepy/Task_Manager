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
        try
        {
            manager.Load();
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            ShowMessage("Не удалось прочитать файл с задачами:\n" + ex.Message, MessageBoxIcon.Error);
        }

        RefreshList();
        SetNextId();
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
        colDescription.Width = -2;
        lblCount.Text = "Задач: " + manager.Count;
    }

    private void SetNextId()
    {
        numId.Value = manager.GetNextId();
    }

    private void SaveTasks()
    {
        try
        {
            manager.Save();
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            ShowMessage("Не удалось сохранить задачи в файл:\n" + ex.Message, MessageBoxIcon.Error);
        }
    }

    private void ShowMessage(string text, MessageBoxIcon icon = MessageBoxIcon.Warning)
    {
        MessageBox.Show(text, AppTitle, MessageBoxButtons.OK, icon);
    }

    private void DeleteSelected()
    {
        if (listTasks.SelectedItems.Count == 0)
        {
            ShowMessage("Сначала выберите задачи в списке.");
            return;
        }

        foreach (ListViewItem item in listTasks.SelectedItems)
        {
            manager.Remove((int)item.Tag!);
        }

        SaveTasks();
        RefreshList();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        string description = txtDescription.Text.Trim();
        int id = (int)numId.Value;

        if (description == "")
        {
            ShowMessage("Введите описание задачи.");
            txtDescription.Focus();
            return;
        }

        if (manager.Contains(id))
        {
            DialogResult answer = MessageBox.Show(
                $"Задача с ID {id} уже есть. Заменить её описание?", AppTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;
        }

        manager.Add(id, description);
        SaveTasks();

        txtDescription.Clear();
        RefreshList();
        SetNextId();
        txtDescription.Focus();
    }

    private void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        DeleteSelected();
    }

    private void btnDeleteIds_Click(object sender, EventArgs e)
    {
        RemoveResult result = manager.RemoveByIds(txtDeleteIds.Text);

        if (result.IsEmpty)
        {
            ShowMessage("Введите ID задач через запятую, например: 1, 2, 5");
            return;
        }

        if (result.Removed.Count > 0)
        {
            SaveTasks();
            RefreshList();
        }

        txtDeleteIds.Clear();

        // если всё удалилось без проблем, сообщение не нужно
        if (result.NotFound.Count == 0 && result.Invalid.Count == 0)
            return;

        var lines = new List<string>();

        if (result.Removed.Count > 0)
            lines.Add("Удалено: " + string.Join(", ", result.Removed));

        if (result.NotFound.Count > 0)
            lines.Add("Не найдено: " + string.Join(", ", result.NotFound));

        if (result.Invalid.Count > 0)
            lines.Add("Неверный ID: " + string.Join(", ", result.Invalid));

        ShowMessage(string.Join(Environment.NewLine, lines));
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        if (manager.Count == 0)
        {
            ShowMessage("Список и так пустой.");
            return;
        }

        DialogResult answer = MessageBox.Show("Точно очистить весь список?", AppTitle,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (answer != DialogResult.Yes)
            return;

        manager.Clear();
        SaveTasks();
        RefreshList();
        SetNextId();
    }

    private void listTasks_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete && listTasks.SelectedItems.Count > 0)
            DeleteSelected();
    }

    private void listTasks_Resize(object sender, EventArgs e)
    {
        colDescription.Width = -2;
    }

    // пока курсор в поле удаления, Enter должен удалять, а не добавлять
    private void txtDeleteIds_Enter(object sender, EventArgs e)
    {
        AcceptButton = btnDeleteIds;
    }

    private void txtDeleteIds_Leave(object sender, EventArgs e)
    {
        AcceptButton = btnAdd;
    }
}
