namespace ToDoApp.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; } = 1; // 0 = Low, 1 = Medium, 2 = High
    public string? Category { get; set; }
    public int EstimatedTimeMinutes { get; set; }

}