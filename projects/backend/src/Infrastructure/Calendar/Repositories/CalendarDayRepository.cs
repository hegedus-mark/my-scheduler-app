using Application.Calendar.Interfaces.Repositories;
using Domain.Calendar.Models.CalendarDays;
using Infrastructure.Calendar.Entities;
using Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar.Repositories;

internal class CalendarDayRepository
    : BaseRepository<CalendarDay, CalendarDayEntity>,
        ICalendarDayRepository
{
    public CalendarDayRepository(DbContext context)
        : base(context) { }

    protected override CalendarDay MapToDomain(CalendarDayEntity entity)
    {
        throw new NotImplementedException();
    }

    protected override CalendarDayEntity MapToEntity(CalendarDay domain)
    {
        throw new NotImplementedException();
    }

    protected override void MapToExistingEntity(CalendarDay domain, CalendarDayEntity entity)
    {
        throw new NotImplementedException();
    }
}
