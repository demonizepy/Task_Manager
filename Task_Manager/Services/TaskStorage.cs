using Task_Manager.Models;

namespace Task_Manager.Services;

public class TaskStorage
{
    private const char Separator = '|';

    private readonly string filePath;

    public TaskStorage(string filePath)
    {
        this.filePath = filePath;
    }

    public List<TaskItem> Load()
    {
        var result = new List<TaskItem>();

        if (!File.Exists(filePath))
            return result;

        foreach (string line in File.ReadAllLines(filePath))
        {
            // раньше после | писался пробел, его надо обрезать
            string[] parts = line.Split(Separator, 2);

            if (parts.Length < 2 || !int.TryParse(parts[0], out int id))
                continue;

            result.Add(new TaskItem(id, parts[1].Trim()));
        }

        return result;
    }

    public void Save(IEnumerable<TaskItem> tasks)
    {
        var lines = tasks.Select(t => $"{t.Id}{Separator}{t.Description}");
        File.WriteAllLines(filePath, lines);
    }
}
