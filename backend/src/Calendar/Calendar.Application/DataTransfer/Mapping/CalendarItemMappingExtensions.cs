using Calendar.Application.DataTransfer.DTOs;
using Calendar.Application.DataTransfer.DTOs.Enums;
using Calendar.Domain.Enums;
using Calendar.Domain.Models.CalendarDays;
using Calendar.Domain.Models.CalendarItems;
using Calendar.Domain.Models.ExternalReferenceItem;
using SharedKernel.Domain.ValueObjects;
using SharedKernel.Extensions;

namespace Calendar.Application.DataTransfer.Mapping;

public static class CalendarItemMappingExtensions
{
    public static CalendarItemDto ToDto(this CalendarItem calendarItem, CalendarDay day)
    {
        ExternalItemType? externalType = null;
        Guid? externalId = null;

        if (calendarItem is CalendarItemWithExternalReference externalItem)
        {
            externalId = externalItem.ExternalId;
            externalType = externalItem.ExternalItemType;
        }

        return new CalendarItemDto
        {
            Id = calendarItem.Id,
            CalendarDayId = day.Id,
            StartTime = calendarItem.TimeSlot.Start.ToDateTime(day.Date),
            EndTime = calendarItem.TimeSlot.End.ToDateTime(day.Date),
            Title = calendarItem.Title,
            RecurrencePattern = calendarItem is RecurringCalendarItem e
                ? e.RecurrencePattern.ToDto()
                : null,
            ExternalItemType = (ExternalItemTypeDto)externalType,
            ExternalId = externalId,
        };
    }

    public static CalendarItem ToDomain(this CalendarItemDto dto)
    {
        return dto switch
        {
            { ExternalId: not null, ExternalItemType: not null } =>
                CalendarItemWithExternalReference.Load(
                    dto.ExternalId.Value,
                    (ExternalItemType)dto.ExternalItemType.Value,
                    TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                    dto.Title,
                    dto.Id
                ),
            { RecurrencePattern: not null } => RecurringCalendarItem.Load(
                dto.Id,
                TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                dto.Title,
                dto.RecurrencePattern.ToDomain()
            ),
            _ => CalendarItem.Load(
                dto.Id,
                TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                dto.Title
            ),
        };
    }
}
