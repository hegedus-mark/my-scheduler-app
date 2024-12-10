using Scheduler.Core.Enum;

namespace Scheduler.Core.Models;

public class ScheduledTask : CalendarItem
{
    public UnscheduledTask OriginalTask { get; }

    public ScheduledTask(UnscheduledTask task, TimeSlot assignedTimeSlot) : base(assignedTimeSlot)
    {
        if (assignedTimeSlot.Duration != task.Duration)
        {
            throw new ArgumentException(
                "Assigned time slot duration must match task duration",
                nameof(assignedTimeSlot));
        }

        if (assignedTimeSlot.End > task.DueDate)
        {
            throw new ArgumentException(
                "Cannot schedule task after its due date",
                nameof(assignedTimeSlot));
        }

        OriginalTask = task;
    }

    // Convenience properties to access original task information
    public string Name => OriginalTask.Name;
    public int Score => OriginalTask.Score;
    public DateTime DueDate => OriginalTask.DueDate;
    public PriorityLevel Priority => OriginalTask.PriorityLevel;
}