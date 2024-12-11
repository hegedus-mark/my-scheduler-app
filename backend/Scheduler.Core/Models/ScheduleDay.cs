using Scheduler.Core.Extensions;
using Scheduler.Core.Models.CalendarItems;

namespace Scheduler.Core.Models;

public class ScheduleDay : EntityBase
{
    //TODO: Move these to appsettings
    private static readonly TimeSpan MinTaskTime = new(0, 30, 0);
    private static readonly TimeSpan MinEventTime = new(0, 1, 0);

    private readonly List<CalendarItem> _calendarItems;
    private readonly List<TimeSlot> _freeSlots;

    public ScheduleDay(DateOnly dayDate, TimeSlot workingHours)
        : base(Guid.NewGuid())
    {
        DayDate = dayDate; // Set DayDate to midnight

        WorkingHours = workingHours;
        _calendarItems = new List<CalendarItem>();
        _freeSlots = [TimeSlot.Create(workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public ScheduleDay(DateOnly dayDate, TimeSlot workingHours, Guid id)
        : base(id)
    {
        DayDate = dayDate;

        WorkingHours = workingHours;
        _calendarItems = new List<CalendarItem>();
        _freeSlots = [TimeSlot.Create(workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public DateOnly DayDate { get; }
    public TimeSlot WorkingHours { get; }
    public IReadOnlyList<CalendarItem> CalendarItems => _calendarItems;
    public IReadOnlyList<TimeSlot> FreeSlots => _freeSlots;

    public void AddEvent(Event eventItem)
    {
        _calendarItems.Add(eventItem);
        ReCalculateFreeSlots(eventItem);
    }

    public ScheduledTask AddScheduledTask(TaskItem taskToSchedule, TimeSlot proposedTimeSlot)
    {
        if (!IsBeforeDueDate(taskToSchedule.DueDate, proposedTimeSlot))
            throw new InvalidOperationException("Scheduled date is after task due date");

        ValidateTimeSlot(proposedTimeSlot);

        var scheduledTask = new ScheduledTask(taskToSchedule, proposedTimeSlot);

        _calendarItems.Add(scheduledTask);
        ReCalculateFreeSlots(scheduledTask);

        return scheduledTask;
    }

    private void ReCalculateFreeSlots(CalendarItem item)
    {
        var placedTimeSlot = item.TimeSlot;

        var parentFreeSlot = FindContainingSlot(placedTimeSlot);

        _freeSlots.Remove(parentFreeSlot);

        if (parentFreeSlot.Start < placedTimeSlot.Start)
        {
            var firstHalfSlot = TimeSlot.Create(parentFreeSlot.Start, placedTimeSlot.Start);
            _freeSlots.Add(firstHalfSlot);
        }

        if (parentFreeSlot.End > placedTimeSlot.End)
        {
            var secondHalfSlot = TimeSlot.Create(placedTimeSlot.End, parentFreeSlot.End);
            _freeSlots.Add(secondHalfSlot);
        }
    }

    private TimeSlot FindContainingSlot(TimeSlot timeSlotToPlace)
    {
        foreach (var freeSlot in _freeSlots)
            if (freeSlot.Contains(timeSlotToPlace))
                return freeSlot;

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

    private void ValidateTimeSlot(TimeSlot timeSlot)
    {
        if (!WorkingHours.Contains(timeSlot))
            throw new InvalidOperationException("Time slot must be within working hours");

        if (DoesTimeSlotOverlap(timeSlot))
            throw new InvalidOperationException(
                "Time slot overlaps with an existing calendar item"
            );
    }

    public bool DoesTimeSlotOverlap(TimeSlot timeSlot, CalendarItem itemToExclude = null)
    {
        return _calendarItems
            .Where(item => item != itemToExclude)
            .Any(item => item.TimeSlot.Overlaps(timeSlot));
    }

    private bool IsBeforeDueDate(DateTime dueDate, TimeSlot timeSlot)
    {
        return dueDate > DayDate.ToDateTime().AddMinutes(timeSlot.End.Minute);
    }
}
