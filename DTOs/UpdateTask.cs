public class UpdateTaskDto
{
    public string? Title { get; init; }
    public bool? IsCompleted { get; init; }
    public DateTime? DueDate { get; init; }
    public string? Description { get; init; }
    public int? Priority { get; init; }            // 0..2
    public string? Category { get; init; }
    public int? EstimatedTimeMinutes { get; init; }
}