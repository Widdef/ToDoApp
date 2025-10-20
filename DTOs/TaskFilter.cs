namespace ToDoApp.DTOs;
public class TaskQuery
{
    public string? Q { get; init; }
    public bool? Completed { get; init; }
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }

    public string? SortBy { get; init; } = "createdAt";
    public string? Order { get; init; } = "desc";

    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 20;
}