using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Models.CalendarItems;

public class TaskReservation : CalendarItem
{
    private TaskReservation(TimeSlot timeSlot, string title, Guid taskId, Guid? id = null)
        : base(timeSlot, title, id) { }

    public Guid TaskId { get; }

    public static TaskReservation Create(TimeSlot timeSlot, string title, Guid taskId)
    {
        return new TaskReservation(timeSlot, title, taskId);
    }

    public static TaskReservation Load(TimeSlot timeSlot, string title, Guid taskId, Guid id)
    {
        return new TaskReservation(timeSlot, title, taskId, id);
    }
}
