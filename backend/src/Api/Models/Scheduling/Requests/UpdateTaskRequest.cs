using Domain.Scheduling.Models.Enums;

namespace Api.Models.Scheduling.Requests;

public record UpdateTaskRequest(
    Guid TaskId,
    string? Name = null,
    DateTime? DueDate = null,
    TimeSpan? Duration = null,
    PriorityLevel? Priority = null
);
