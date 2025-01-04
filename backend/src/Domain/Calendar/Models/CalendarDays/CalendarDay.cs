using Domain.Calendar.Events;
using Domain.Calendar.Models.CalendarItems;
using SharedKernel.Common.Errors;
using SharedKernel.Common.Results;
using SharedKernel.Domain.Base;
using SharedKernel.Domain.ValueObjects;

namespace Domain.Calendar.Models.CalendarDays;

public abstract class CalendarDay : AggregateRoot
{
    private readonly List<CalendarItem> _items = new();

    protected CalendarDay(DateOnly date, Guid? id = null)
        : base(id)
    {
        Date = date;
    }

    public DateOnly Date { get; }

    public abstract bool IsWorkingDay { get; }

    public IReadOnlyCollection<CalendarItem> Items => _items.AsReadOnly();

    public virtual Result<CalendarItem> AddItem(CalendarItem item)
    {
        if (HasTimeConflict(item.TimeSlot))
            return Result<CalendarItem>.Failure(
                new Error("TimeConflict", "Item conflicts with existing items")
            );

        _items.Add(item);
        AddDomainEvent(new CalendarItemAddedEvent(Id, item.Id));

        return Result<CalendarItem>.Success(item);
    }

    public virtual Result<CalendarItem> RescheduleItem(Guid itemId, TimeSlot newSlot)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            return Result<CalendarItem>.Failure(
                new Error("ItemNotFound", "Calendar item not found")
            );

        if (HasTimeConflict(newSlot, item))
            return Result<CalendarItem>.Failure(
                new Error("TimeConflict", "New time slot conflicts with existing items")
            );

        var updateResult = item.UpdateTimeSlot(newSlot);
        if (updateResult.IsSuccess)
            AddDomainEvent(new CalendarItemRescheduledEvent(Id, item.Id, newSlot));

        return updateResult;
    }

    protected bool HasTimeConflict(TimeSlot timeSlot, CalendarItem? excludeItem = null)
    {
        return _items
            .Where(item => !Equals(item, excludeItem))
            .Any(item => item.TimeSlot.Overlaps(timeSlot));
    }
}
