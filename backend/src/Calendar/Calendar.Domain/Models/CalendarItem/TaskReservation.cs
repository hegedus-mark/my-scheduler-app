using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Models.CalendarItem;

public class TaskReservation : CalendarItem
{
    private TaskReservation(TimeSlot timeSlot, string title, Guid? id = null)
        : base(timeSlot, title, id) { }

    public static TaskReservation Create(TimeSlot timeSlot, string title)
    {
        return new TaskReservation(timeSlot, title);
    }

    public static TaskReservation Load(TimeSlot timeSlot, string title, Guid id)
    {
        return new TaskReservation(timeSlot, title, id);
    }
}
