using Scheduling.Application.DataTransfer.DTOs.Enums;

namespace Scheduling.Application.DataTransfer.DTOs;

public class TaskItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public DateTime DueDate { get; init; }
    public TimeSpan Duration { get; init; }
    public PriorityLevelDto PriorityLevel { get; init; }
    public TaskItemStatusDto TaskItemStatus { get; init; }

    // For scheduled state
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    // For failed state
    public string? FailureReason { get; init; }
}
