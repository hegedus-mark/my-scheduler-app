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
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // For failed state
    public string? FailureReason { get; set; }

    public Guid Id { get; init; }
}
