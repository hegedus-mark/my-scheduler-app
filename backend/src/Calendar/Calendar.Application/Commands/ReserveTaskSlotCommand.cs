namespace Calendar.Application.Commands;

public class ReserveTaskSlotCommand
{
    public ReserveTaskSlotCommand(
        string taskTitle,
        Guid taskId,
        DateTime startTime,
        DateTime endTime
    )
    {
        TaskTitle = taskTitle;
        TaskId = taskId;
        StartTime = startTime;
        EndTime = endTime;
    }

    public Guid TaskId { get; init; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string TaskTitle { get; init; }
}
