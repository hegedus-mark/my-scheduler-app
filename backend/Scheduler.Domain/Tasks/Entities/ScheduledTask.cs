using Scheduler.Domain.Shared;
using Scheduler.Domain.Shared.Enums;

namespace Scheduler.Domain.Tasks.Entities;

public class ScheduledTask : CalendarItem
{
    //For new ScheduledTask
    public ScheduledTask(TaskItem task, TimeSlot assignedTimeSlot)
        : base(assignedTimeSlot)
    {
        if (assignedTimeSlot.Duration != task.Duration)
            throw new ArgumentException(
                "Assigned time slot duration must match task duration",
                nameof(assignedTimeSlot)
            );

        OriginalTask = task;
    }

    //For existing ScheduledTask
    public ScheduledTask(Guid id, TaskItem task, TimeSlot timeSlot)
        : base(id, timeSlot)
    {
        OriginalTask = task;
    }

    public TaskItem OriginalTask { get; }

    // Convenience properties to access original task information
    public string Name => OriginalTask.Name;
    public int Score => OriginalTask.Score;
    public DateTime DueDate => OriginalTask.DueDate;
    public PriorityLevel Priority => OriginalTask.PriorityLevel;
}
