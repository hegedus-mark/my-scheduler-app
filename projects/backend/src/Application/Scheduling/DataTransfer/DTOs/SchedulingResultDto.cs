namespace Application.Scheduling.DataTransfer.DTOs;

public class SchedulingResultDto
{
    public List<TaskItemDto> ScheduledTasks { get; init; } = new();
    public List<TaskItemDto> FailedTasks { get; init; } = new();

    public required bool HasFailedTasks { get; init; }
}
