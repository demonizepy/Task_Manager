using Task_Manager.Models;

namespace Task_Manager.Services;

public class TaskManager
{
    private readonly Dictionary<int, TaskItem> tasks = new();
    private readonly TaskStorage storage;

    public TaskManager(TaskStorage storage)
    {
        this.storage = storage;
    }

    public int Count => tasks.Count;

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

    public RemoveResult RemoveByIds(string input)
    {
        var result = new RemoveResult();

        foreach (string part in input.Split(','))
        {
            string text = part.Trim();

            if (text == "")
                continue;

            if (!int.TryParse(text, out int id))
                result.Invalid.Add(text);
            else if (tasks.Remove(id))
                result.Removed.Add(id);
            else
                result.NotFound.Add(id);
        }

        return result;
    }

    public void Clear()
    {
        tasks.Clear();
    }

    public void Load()
    {
        foreach (TaskItem task in storage.Load())
        {
            tasks[task.Id] = task;
        }
    }

    public void Save()
    {
        storage.Save(tasks.Values);
    }
}
