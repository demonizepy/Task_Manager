namespace Task_Manager;

public class TaskManager
{
    private readonly Dictionary<int, TaskItem> tasks = new();
    private readonly string filePath;

    public TaskManager(string filePath)
    {
        this.filePath = filePath;
    }

    public int Count
    {
        get { return tasks.Count; }
    }

    public List<TaskItem> GetAll()
    {
        return tasks.Values.OrderBy(t => t.Id).ToList();
    }

    // true - задача новая, false - заменили уже существующую
    public bool Add(int id, string description)
    {
        bool isNew = !tasks.ContainsKey(id);
        tasks[id] = new TaskItem(id, description);
        return isNew;
    }

    public bool Remove(int id)
    {
        return tasks.Remove(id);
    }

    public void Clear()
    {
        tasks.Clear();
    }

    public void Save()
    {
        var lines = tasks.Values.Select(t => $"{t.Id}|{t.Description}");
        File.WriteAllLines(filePath, lines);
    }

    public void Load()
    {
        if (!File.Exists(filePath))
            return;

        foreach (string line in File.ReadAllLines(filePath))
        {
            // раньше после | писался пробел, его надо обрезать
            string[] parts = line.Split('|', 2);

            if (parts.Length < 2 || !int.TryParse(parts[0], out int id))
                continue;

            tasks[id] = new TaskItem(id, parts[1].Trim());
        }
    }
}
