using Scheduler.Domain.Calendars.Events;
using Scheduler.Domain.Calendars.Interfaces;
using Scheduler.Domain.Shared;

namespace Scheduler.Domain.Calendars.Entities;

public class NonWorkingDay : EntityBase, ICalendarDay
{
    private readonly List<CalendarItem> _calendarItems;

    private NonWorkingDay(Guid? id, DateOnly dayDate)
        : base(id)
    {
        DayDate = dayDate;
        _calendarItems = new List<CalendarItem>();
    }

    public DateOnly DayDate { get; }

    public bool IsWorkingDay => false;

    public IReadOnlyList<CalendarItem> CalendarItems => _calendarItems;

    public void AddEvent(Event eventItem)
    {
        _calendarItems.Add(eventItem);
    }

    public static NonWorkingDay Create(DateOnly dayDate)
    {
        return new NonWorkingDay(null, dayDate);
    }

    public static NonWorkingDay Load(Guid id, DateOnly dayDate)
    {
        return new NonWorkingDay(id, dayDate);
    }
}
