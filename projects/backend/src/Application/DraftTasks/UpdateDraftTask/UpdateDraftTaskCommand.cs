namespace Application.DraftTasks.UpdateDraftTask;

public record UpdateDraftTaskCommand
{
    public Guid DraftTaskId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public TimeSpan? Duration { get; init; } 
    public Priority? Priority { get; init; }
    public DateTime? Deadline { get; init; }
}