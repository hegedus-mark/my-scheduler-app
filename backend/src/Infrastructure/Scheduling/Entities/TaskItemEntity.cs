using System.ComponentModel.DataAnnotations;
using Domain.Scheduling.Models.Enums;
using SharedKernel.Persistence;

namespace Infrastructure.Scheduling.Entities;

public class TaskItemEntity : IEntity
{
    [Required]
    public string Name { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public PriorityLevel PriorityLevel { get; set; }

    [Required]
    public TaskItemStatus TaskItemStatus { get; set; }

    // For scheduled state
    public DateTime? StartDate { get; init; } = null;
    public DateTime? EndDate { get; init; } = null;

    // For failed state
    public string? FailureReason { get; init; }

    [Required]
    public Guid Id { get; init; }
}
