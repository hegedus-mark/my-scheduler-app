namespace Scheduler.Core.Models;

public abstract class CalendarItem
{
    public TimeSlot TimeSlot { get; set; }
    
    protected CalendarItem(TimeSlot timeSlot)
    {
        TimeSlot = timeSlot;
    }
}