using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Base;

namespace Scheduler.Domain.Interfaces;

public interface ICalendarDay : IIdModel
{
    /// <summary>
    ///     Gets the date of this schedule day.
    /// </summary>
    DateOnly DayDate { get; }

    /// <summary>
    ///     Gets whether the date is a Working day.
    /// </summary>
    bool IsWorkingDay { get; }

    /// <summary>
    ///     Gets the list of all calendar items scheduled for this day.
    /// </summary>
    IReadOnlyList<CalendarItem> CalendarItems { get; }

    /// <summary>
    ///     Adds an event to this day and updates available time slots.
    /// </summary>
    /// <param name="eventItem">The event to add</param>
    void AddEvent(Event eventItem);
}
