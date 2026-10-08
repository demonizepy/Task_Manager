namespace Task_Manager.Models;

public class RemoveResult
{
    public List<int> Removed { get; } = new();
    public List<int> NotFound { get; } = new();
    public List<string> Invalid { get; } = new();

    public bool IsEmpty => Removed.Count == 0 && NotFound.Count == 0 && Invalid.Count == 0;
}
