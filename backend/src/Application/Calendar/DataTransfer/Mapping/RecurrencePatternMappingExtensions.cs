using Application.Calendar.DataTransfer.DTOs;
using Calendar.Domain.Models.Enums;
using Calendar.Domain.ValueObjects;

namespace Application.Calendar.DataTransfer.Mapping;

public static class RecurrencePatternMappingExtensions
{
    public static RecurrencePatternDto ToDto(this RecurrencePattern pattern)
    {
        return new RecurrencePatternDto
        {
            RecurrenceType = pattern.Type.ToString(),
            Interval = pattern.Interval,
            EndDate = pattern.EndDate,
            SelectedDays = pattern.SelectedDays,
        };
    }

    public static RecurrencePattern ToDomain(this RecurrencePatternDto dto)
    {
        var recurrenceType = Enum.Parse<RecurrenceType>(dto.RecurrenceType);

        return recurrenceType switch
        {
            RecurrenceType.Daily => RecurrencePattern.CreateDaily(dto.Interval, dto.EndDate),
            RecurrenceType.Weekly => RecurrencePattern.CreateWeekly(
                dto.Interval,
                dto.SelectedDays
                    ?? throw new ArgumentException("Selected days required for weekly recurrence"),
                dto.EndDate
            ),
            _ => throw new NotSupportedException(
                $"Unsupported recurrence type: {dto.RecurrenceType}"
            ),
        };
    }
}
