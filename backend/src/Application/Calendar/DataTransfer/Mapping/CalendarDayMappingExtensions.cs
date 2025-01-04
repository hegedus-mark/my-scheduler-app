using Application.Calendar.DataTransfer.DTOs;
using Domain.Calendar.Models.CalendarDays;
using SharedKernel.Domain.ValueObjects;
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
            IsWorkingDay = day.IsWorkingDay,
            WorkStartTime = null,
            WorkEndTime = null,
            Reservations = day.Items.Select(i => i.ToDto(day)).ToList(),
        };

        if (day is WorkingDay wd)
        {
            dto.WorkStartTime = wd.WorkingHours.Start.ToDateTime(wd.Date);
            dto.WorkEndTime = wd.WorkingHours.End.ToDateTime(wd.Date);
        }

        return dto;
    }

    public static CalendarDay ToDomain(this CalendarDayDto dto)
    {
        if (dto.IsWorkingDay)
        {
            var day = WorkingDay.Load(
                dto.Date.ToDateOnly(),
                TimeSlot.Create(
                    dto.WorkStartTime!.Value.ToTimeOnly(),
                    dto.WorkEndTime!.Value.ToTimeOnly()
                ),
                dto.Id
            );

            foreach (var reservation in dto.Reservations)
                day.AddItem(reservation.ToDomain());

            return day;
        }
        else
        {
            var day = NonWorkingDay.Load(dto.Id, dto.Date.ToDateOnly());

            foreach (var reservation in dto.Reservations)
                day.AddItem(reservation.ToDomain());

            return day;
        }
    }
}
