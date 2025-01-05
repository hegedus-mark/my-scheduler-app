using Application.Shared.Contracts;
using Domain.Scheduling.Models.Enums;

namespace Infrastructure.Scheduling.Entities;

public class TaskItemEntity : IEntity
{
    public string Name { get; set; }

    public DateTime DueDate { get; set; }

    public TimeSpan Duration { get; set; }

    public PriorityLevel PriorityLevel { get; set; }

    public TaskItemStatus TaskItemStatus { get; set; }

    // For scheduled state
    public DateTime? StartDate { get; init; } = null;
    public DateTime? EndDate { get; init; } = null;

    // For failed state
    public string? FailureReason { get; init; }

    public Guid Id { get; init; }
}
