using Calendar.Application.DTOs;
using Calendar.Domain.Models.CalendarDays;
using Calendar.Domain.Models.CalendarItems;
using SharedKernel.Domain.ValueObjects;
using SharedKernel.Extensions;

namespace Calendar.Application.Mapping;

public static class CalendarItemMappingExtensions
{
    public static CalendarItemDto ToDto(this CalendarItem calendarItem, CalendarDay day)
    {
        //TODO: create enum for this or smth
        var type = calendarItem switch
        {
            Event => "Event",
            TaskReservation => "TaskReservation",
            _ => throw new NotSupportedException(),
        };

        return new CalendarItemDto
        {
            Id = calendarItem.Id,
            CalendarDayId = day.Id,
            Type = type,
            StartTime = calendarItem.TimeSlot.Start.ToDateTime(day.Date),
            EndTime = calendarItem.TimeSlot.End.ToDateTime(day.Date),
            Title = calendarItem.Title,
            RecurrencePattern = calendarItem is Event e ? e.RecurrencePattern.ToDto() : null,
        };
    }

    public static CalendarItem ToDomain(this CalendarItemDto dto)
    {
        switch (dto.Type)
        {
            case "Event":
                return Event.Load(
                    dto.Id,
                    TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                    dto.Title,
                    dto.RecurrencePattern!.ToDomain()
                );
            case "TaskReservation":
                return TaskReservation.Load(
                    TimeSlot.Create(dto.StartTime.ToTimeOnly(), dto.EndTime.ToTimeOnly()),
                    dto.Title,
                    dto.ExternalTaskId!.Value,
                    dto.Id
                );
            default:
                throw new InvalidOperationException("Unknown dto type");
        }
    }
}
