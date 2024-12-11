namespace Scheduler.Core.Models;

public class ScheduleCalendar : EntityBase
{
    private readonly SortedDictionary<DateOnly, ScheduleDay> _days;

    //new Calendar
    public ScheduleCalendar()
        : base(Guid.NewGuid()) { }

    //existing Calendar
    public ScheduleCalendar(Guid id)
        : base(id) { }
}
