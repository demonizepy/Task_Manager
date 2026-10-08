namespace Task_Manager;

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
        return $"ID: {Id}, описание: {Description}";
    }
}
