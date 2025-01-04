using Domain.Scheduling.Models.Enums;

namespace Application.Scheduling.DataTransfer.DTOs;

public class TaskItemDto
{
    public Guid? Id { get; init; }

    public required string Name { get; init; }

    public required DateTime DueDate { get; init; }

    public required TimeSpan Duration { get; init; }

    public required PriorityLevel PriorityLevel { get; init; }

    public required TaskItemStatus TaskItemStatus { get; init; }

    // For scheduled state
    public DateTime? StartDate { get; init; } = null;
    public DateTime? EndDate { get; init; } = null;

    // For failed state
    public string? FailureReason { get; init; }
}
