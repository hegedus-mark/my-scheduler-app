namespace Scheduling.Application.DataTransfer.DTOs;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public TimeSpan Duration { get; set; }
    public string Priority { get; set; } = null!;
    public string State { get; set; } = null!;

    // For scheduled state
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // For failed state
    public string? FailureReason { get; set; }
}
