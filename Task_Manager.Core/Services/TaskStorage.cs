using Task_Manager.Core.Models;

namespace Task_Manager.Core.Services;

public class TaskStorage
{
    private const char Separator = '|';

    public TaskStorage(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    public List<TaskItem> Load()
    {
        var result = new List<TaskItem>();

        if (!File.Exists(FilePath))
            return result;

        foreach (string line in File.ReadAllLines(FilePath))
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
        File.WriteAllLines(FilePath, lines);
    }
}
