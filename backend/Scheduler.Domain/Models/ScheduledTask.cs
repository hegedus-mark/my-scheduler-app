using Scheduler.Domain.Models.Base;
using Scheduler.Shared.Enums;
using Scheduler.Shared.ValueObjects;

namespace Scheduler.Domain.Models;

public class ScheduledTask : CalendarItem
{
    private ScheduledTask(Guid? id, TaskItem task, TimeSlot timeSlot)
        : base(timeSlot, id)
    {
        if (timeSlot.Duration != task.Duration)
            throw new ArgumentException(
                "Assigned time slot duration must match task duration",
                nameof(timeSlot)
            );

        OriginalTask = task;
    }

    public TaskItem OriginalTask { get; }

    // Convenience properties to access original task information
    public string Name => OriginalTask.Name;
    public int Score => OriginalTask.Score;
    public DateTime DueDate => OriginalTask.DueDate;
    public PriorityLevel Priority => OriginalTask.PriorityLevel;

    public static ScheduledTask Create(TaskItem task, TimeSlot timeSlot)
    {
        return new ScheduledTask(null, task, timeSlot);
    }

    public static ScheduledTask Load(Guid id, TaskItem task, TimeSlot timeSlot)
    {
        return new ScheduledTask(id, task, timeSlot);
    }
}
