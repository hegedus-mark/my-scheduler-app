using Calendar.Domain.Models.CalendarItems;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Domain.Models.CalendarDays;

public class WorkingDay : CalendarDay
{
    private readonly List<TimeSlot> _freeSlots;

    private WorkingDay(Guid? id, DateOnly dayDate, TimeSlot workingHours)
        : base(dayDate, id)
    {
        WorkingHours = workingHours;
        _freeSlots = [TimeSlot.Create(workingHours.Start, workingHours.End)]; // At first the entire day is free
    }

    public TimeSlot WorkingHours { get; }

    /// <summary>
    ///     Gets the list of available time slots in this day.
    /// </summary>


    public override bool IsWorkingDay => true;

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
    public static WorkingDay Load(DateOnly dayDate, TimeSlot workingHours, Guid id)
    {
        return new WorkingDay(id, dayDate, workingHours);
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
        return Items
            .Where(item => !Equals(item, itemToExclude))
            .Any(item => item.TimeSlot.Overlaps(timeSlot));
    }
}
