using Application.Calendar.DataTransfer.DTOs;
using Domain.Calendar.Models.CalendarDays;
using SharedKernel.Extensions;

namespace Application.Calendar.DataTransfer.Mapping;

public static class CalendarDayMappingExtensions
{
    public static CalendarDayDto ToDto(this CalendarDay day)
    {
        var dto = new CalendarDayDto
        {
            Id = day.Id,
            Date = day.Date.ToDateTime(TimeOnly.MinValue),
            WorkStartTime = null,
            WorkEndTime = null,
            Reservations = day.Items.Select(i => i.ToDto(day)).ToList(),
        };

        return dto;
    }

    public static CalendarDay ToDomain(this CalendarDayDto dto)
    {
        if (dto.IsWorkingDay)
        {
            var day = CalendarDay.Load(dto.Id, dto.Date.ToDateOnly());

            foreach (var reservation in dto.Reservations)
                day.AddItem(reservation.ToDomain());

            return day;
        }
        else
        {
            var day = CalendarDay.Load(dto.Id, dto.Date.ToDateOnly());

            foreach (var reservation in dto.Reservations)
                day.AddItem(reservation.ToDomain());

            return day;
        }
    }
}
