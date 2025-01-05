using Domain.Calendar.Events;
using Domain.Calendar.Exceptions;
using Domain.Calendar.Models.CalendarItems;
using SharedKernel.Results;
using AggregateRoot = Domain.Shared.Base.AggregateRoot;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

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

    public virtual void AddItem(CalendarItem item)
    {
        if (HasTimeConflict(item.TimeSlot))
            throw new TimeConflictException();

        _items.Add(item);
        AddDomainEvent(new CalendarItemAddedEvent(Id, item.Id));
    }

    public virtual Result<CalendarItem> RescheduleItem(Guid itemId, TimeSlot newSlot)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new CalendarItemNotFoundException(itemId);

        if (HasTimeConflict(newSlot, item))
            throw new TimeConflictException();
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
