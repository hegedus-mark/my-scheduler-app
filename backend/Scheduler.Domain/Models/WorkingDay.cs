using Scheduler.Domain.Calendars.Interfaces;
using Scheduler.Domain.Models.Base;
using Scheduler.Domain.Shared;
using Scheduler.Shared.Extensions;

namespace Scheduler.Domain.Models;

public class WorkingDay : EntityBase, ICalendarDay
{
    private readonly List<CalendarItem> _calendarItems;
    private readonly List<TimeSlot> _freeSlots;

    private WorkingDay(Guid? id, DateOnly dayDate, TimeSlot workingHours)
        : base(id)
    {
        DayDate = dayDate;
        WorkingHours = workingHours;
        _calendarItems = new List<CalendarItem>();
        _freeSlots = [TimeSlot.Create(workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public TimeSlot WorkingHours { get; }

    /// <summary>
    ///     Gets the list of available time slots in this day.
    /// </summary>
    public IReadOnlyList<TimeSlot> FreeSlots => _freeSlots;

    public bool IsWorkingDay => true;

    public DateOnly DayDate { get; }

    public IReadOnlyList<CalendarItem> CalendarItems => _calendarItems;

    public void AddEvent(Event eventItem)
    {
        _calendarItems.Add(eventItem);
        ReCalculateFreeSlots(eventItem);
    }

    /// <summary>
    ///     Creates a new working day with a new unique identifier.
    /// </summary>
    /// <param name="dayDate">The date for this working day</param>
    /// <param name="workingHours">The time slot defining the working hours for this day</param>
    /// <returns>A new working day instance</returns>
    public static WorkingDay Create(DateOnly dayDate, TimeSlot workingHours)
    {
        return new WorkingDay(null, dayDate, workingHours);
    }

    /// <summary>
    ///     Creates a working day instance with an existing identifier, typically used when loading from storage.
    /// </summary>
    /// <param name="id">The unique identifier of the existing working day</param>
    /// <param name="dayDate">The date for this working day</param>
    /// <param name="workingHours">The time slot defining the working hours for this day</param>
    /// <returns>A working day instance with the specified identifier</returns>
    public static WorkingDay Load(Guid id, DateOnly dayDate, TimeSlot workingHours)
    {
        return new WorkingDay(id, dayDate, workingHours);
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

        var scheduledTask = ScheduledTask.Create(taskToSchedule, proposedTimeSlot);

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

    public IReadOnlyList<Event> GetEvents()
    {
        return _calendarItems.OfType<Event>().ToList();
    }

    public IReadOnlyList<ScheduledTask> GetTasks()
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
    private bool DoesTimeSlotOverlap(TimeSlot timeSlot, CalendarItem itemToExclude = null)
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
