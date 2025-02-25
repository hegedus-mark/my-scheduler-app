namespace Application.DraftTasks.CreateDraftTask;

public record CreateDraftTaskCommand
{
    public string Title { get; init; }
    public string? Description { get; init; }
    public TimeSpan? Duration { get; init; } 
    public Priority Priority { get; init; }
    public DateTime? Deadline { get; init; }
    
}
