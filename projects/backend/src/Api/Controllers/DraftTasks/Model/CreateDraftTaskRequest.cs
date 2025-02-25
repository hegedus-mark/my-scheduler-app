using Application.DraftTasks;

namespace Api.Controllers.DraftTasks.Model;

public record CreateDraftTaskRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TimeSpan? Duration { get; set; }
    public Priority Priority { get; set; }
    public DateTime? Deadline { get; set; }
}