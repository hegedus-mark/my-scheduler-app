using Application.Calendar.DataTransfer.DTOs;
using Domain.Calendar.Models.CalendarDays;
using Domain.Calendar.Models.CalendarItems;
using Domain.Calendar.Models.Enums;
using Domain.Calendar.ValueObjects;
using SharedKernel.Domain.ValueObjects;
using SharedKernel.Extensions;

namespace Application.Calendar.DataTransfer.Mapping;

public static class CalendarItemMappingExtensions
{
    private static RecurrencePattern CreateRecurrencePattern(
        RecurrenceType recurrenceType,
        int interval,
        DateTime? endDate,
        DaysOfWeek? selectedDays
    )
    {
        return recurrenceType switch
        {
            RecurrenceType.Daily => RecurrencePattern.CreateDaily(interval, endDate),
            RecurrenceType.Weekly => RecurrencePattern.CreateWeekly(
                interval,
                selectedDays
                    ?? throw new ArgumentException("Selected days required for weekly recurrence"),
                endDate
            ),
            _ => throw new NotSupportedException($"Unsupported recurrence type: {recurrenceType}"),
        };
    }

    public static CalendarItemDto ToDto(this CalendarItem item, CalendarDay day)
    {
        ExternalItemType? externalType = null;
        Guid? externalId = null;

        if (item is CalendarItemWithExternalReference externalItem)
        {
            externalId = externalItem.ExternalId;
            externalType = externalItem.ExternalItemType;
        }

        RecurrenceType? recurrenceType = null;
        int? recurrenceInterval = null;
        DateTime? recurrenceEndDate = null;
        DaysOfWeek? recurrenceSelectedDays = null;

        if (item is RecurringCalendarItem recurringItem)
        {
            recurrenceType = recurringItem.RecurrencePattern.Type;
            recurrenceInterval = recurringItem.RecurrencePattern.Interval;
            recurrenceEndDate = recurringItem.RecurrencePattern.EndDate;
            recurrenceSelectedDays = recurringItem.RecurrencePattern.SelectedDays;
        }

        return new CalendarItemDto
        {
            Id = item.Id,
            CalendarDayId = day.Id,
            StartTime = item.TimeSlot.Start.ToDateTime(day.Date),
            EndTime = item.TimeSlot.End.ToDateTime(day.Date),
            Title = item.Title,
            RecurrenceType = recurrenceType,
            RecurrenceInterval = recurrenceInterval,
            RecurrenceEndDate = recurrenceEndDate,
            RecurrenceSelectedDays = recurrenceSelectedDays,
            ExternalItemType = externalType,
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
                    dto.ExternalItemType.Value,
                    TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                    dto.Title,
                    dto.Id
                ),
            { RecurrenceType: not null } => RecurringCalendarItem.Load(
                dto.Id,
                TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                dto.Title,
                CreateRecurrencePattern(
                    dto.RecurrenceType
                        ?? throw new ArgumentException(
                            "RecurrenceType can't be null for RecurringCalendarItem"
                        ),
                    dto.RecurrenceInterval
                        ?? throw new ArgumentException("RecurrenceInterval can't be null"),
                    dto.RecurrenceEndDate,
                    dto.RecurrenceSelectedDays
                )
            ),
            _ => CalendarItem.Load(
                dto.Id,
                TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                dto.Title
            ),
        };
    }
}
