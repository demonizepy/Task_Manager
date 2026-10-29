namespace Task_Manager.Core.Models;

public class TaskItem
{
    public int Id { get; }
    public string Description { get; set; }

    public TaskItem(int id, string description)
    {
        Id = id;
        Description = description;
    }

    public override string ToString()
    {
        return $"{Id}: {Description}";
    }
}
