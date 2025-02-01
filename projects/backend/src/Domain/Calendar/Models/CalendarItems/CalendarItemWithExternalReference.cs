using Domain.Calendar.Models.Enums;
using TimeSlot = Domain.Shared.ValueObjects.TimeSlot;

namespace Domain.Calendar.Models.CalendarItems;

public class CalendarItemWithExternalReference : CalendarItem
{
    protected CalendarItemWithExternalReference(
        Guid externalId,
        ExternalItemType externalItemType,
        TimeSlot timeSlot,
        string title,
        Guid? id = null
    )
        : base(timeSlot, title, id)
    {
        ExternalId = externalId;
        ExternalItemType = externalItemType;
    }

    public Guid ExternalId { get; }
    public ExternalItemType ExternalItemType { get; }

    public static CalendarItemWithExternalReference Create(
        Guid externalId,
        ExternalItemType externalItemType,
        TimeSlot timeSlot,
        string title
    )
    {
        return new CalendarItemWithExternalReference(externalId, externalItemType, timeSlot, title);
    }

    public static CalendarItemWithExternalReference Load(
        Guid externalId,
        ExternalItemType externalItemType,
        TimeSlot timeSlot,
        string title,
        Guid id
    )
    {
        return new CalendarItemWithExternalReference(
            externalId,
            externalItemType,
            timeSlot,
            title,
            id
        );
    }
}
