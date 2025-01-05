using Application.Scheduling.DataTransfer.DTOs;

namespace Application.Scheduling.Commands;

public class ScheduleTasksCommand
{
    public ScheduleTasksCommand(
        DateTime? windowStart,
        DateTime? windowEnd,
        IReadOnlyCollection<TaskItemDto> taskItems
    )
    {
        WindowStart = windowStart;
        WindowEnd = windowEnd;
        TaskItems = taskItems;
    }

    public DateTime? WindowStart { get; }
    public DateTime? WindowEnd { get; }
    public IReadOnlyCollection<TaskItemDto> TaskItems { get; }
}
