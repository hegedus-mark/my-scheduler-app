using Scheduler.Core.Models.CalendarItems;

namespace Scheduler.Core.Models;

public class ScheduleDay : EntityBase
{
    //TODO: Move these to appsettings
    private static readonly TimeSpan MinTaskTime = new TimeSpan(0, 30, 0);
    private static readonly TimeSpan MinEventTime = new TimeSpan(0, 1, 0);

    private readonly List<CalendarItem> _calendarItems;
    private readonly List<TimeSlot> _freeSlots;

    public DateOnly DayDate { get; }
    public TimeSlot WorkingHours { get; }
    public IReadOnlyList<CalendarItem> CalendarItems => _calendarItems;
    public IReadOnlyList<TimeSlot> FreeSlots => _freeSlots;

    public ScheduleDay(DateOnly dayDate, TimeSlot workingHours)
        : base(Guid.NewGuid())
    {
        DayDate = dayDate; // Set DayDate to midnight

        if (workingHours.Day != dayDate)
        {
            throw new ArgumentException(
                "StartWork and EndWork must be on the same day as DayDate."
            );
        }

        WorkingHours = workingHours;
        _calendarItems = new List<CalendarItem>();
        _freeSlots = [TimeSlot.Create(dayDate, workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public ScheduleDay(DateOnly dayDate, TimeSlot workingHours, Guid id)
        : base(id)
    {
        DayDate = dayDate;

        if (workingHours.Day != dayDate)
        {
            throw new ArgumentException(
                "StartWork and EndWork must be on the same day as DayDate."
            );
        }

        WorkingHours = workingHours;
        _calendarItems = new List<CalendarItem>();
        _freeSlots = [TimeSlot.Create(dayDate, workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public void AddCalendarItem(CalendarItem calendarItem)
    {
        _calendarItems.Add(calendarItem);
        ReCalculateFreeSlots(calendarItem);
    }

    private void ReCalculateFreeSlots(CalendarItem item)
    {
        var placedTimeSlot = item.TimeSlot;

        TimeSlot parentFreeSlot = FindContainingSlot(placedTimeSlot);

        _freeSlots.Remove(parentFreeSlot);

        if (parentFreeSlot.Start < placedTimeSlot.Start)
        {
            var firstHalfSlot = TimeSlot.Create(
                DayDate,
                parentFreeSlot.Start,
                placedTimeSlot.Start
            );
            _freeSlots.Add(firstHalfSlot);
        }

        if (parentFreeSlot.End > placedTimeSlot.End)
        {
            var secondHalfSlot = TimeSlot.Create(DayDate, placedTimeSlot.End, parentFreeSlot.End);
            _freeSlots.Add(secondHalfSlot);
        }
    }

    private TimeSlot FindContainingSlot(TimeSlot timeSlotToPlace)
    {
        foreach (var freeSlot in _freeSlots)
        {
            if (timeSlotToPlace.IsInsideTimeSlot(freeSlot))
            {
                return freeSlot;
            }
        }

        throw new InvalidOperationException(
            "Added calendarItem is not placed in a free slot, there must be an overlap"
        );
    }

    public IEnumerable<Event> GetEvents()
    {
        return _calendarItems.OfType<Event>().ToList();
    }

    public IEnumerable<ScheduledTask> GetTasks()
    {
        return _calendarItems.OfType<ScheduledTask>().ToList();
    }
}
