using Scheduler.Core.Extensions;
using Scheduler.Core.Models.CalendarItems;

namespace Scheduler.Core.Models;

/// <summary>
///     Represents a single day in the scheduling system, managing calendar items and available time slots.
/// </summary>
public class ScheduleDay : EntityBase
{
    //TODO: Move these to appsettings
    private static readonly TimeSpan MinTaskTime = new(0, 30, 0);
    private static readonly TimeSpan MinEventTime = new(0, 1, 0);

    private readonly List<CalendarItem> _calendarItems;
    private readonly List<TimeSlot> _freeSlots;

    /// <summary>
    ///     Initializes a new day with specified date and working hours.
    /// </summary>
    /// <param name="dayDate">The date of this schedule day</param>
    /// <param name="workingHours">The working hours time slot for this day</param>
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

    /// <summary>
    ///     Gets the date of this schedule day.
    /// </summary>
    public DateOnly DayDate { get; }

    /// <summary>
    ///     Gets the working hours for this day.
    /// </summary>
    public TimeSlot WorkingHours { get; }

    /// <summary>
    ///     Gets the list of all calendar items scheduled for this day.
    /// </summary>
    public IReadOnlyList<CalendarItem> CalendarItems => _calendarItems;

    /// <summary>
    ///     Gets the list of available time slots in this day.
    /// </summary>
    public IReadOnlyList<TimeSlot> FreeSlots => _freeSlots;

    /// <summary>
    ///     Adds an event to this day and updates available time slots.
    /// </summary>
    /// <param name="eventItem">The event to add</param>
    public void AddEvent(Event eventItem)
    {
        _calendarItems.Add(eventItem);
        ReCalculateFreeSlots(eventItem);
    }

    /// <summary>
    ///     Schedules a task for this day within the specified time slot.
    /// </summary>
    /// <param name="taskToSchedule">The task to schedule</param>
    /// <param name="proposedTimeSlot">The proposed time slot for the task</param>
    /// <returns>The scheduled task</returns>
    /// <exception cref="InvalidOperationException">Thrown when the time slot is invalid or overlaps with existing items</exception>
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

    /// <summary>
    ///     Checks if the specified time slot overlaps with existing calendar items.
    /// </summary>
    /// <param name="timeSlot">The time slot to check</param>
    /// <param name="itemToExclude">Optional calendar item to exclude from the check</param>
    /// <returns>True if there is an overlap, false otherwise</returns>
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
