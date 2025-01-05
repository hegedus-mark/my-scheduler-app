using Application.Calendar.Interfaces.Repositories;
using Domain.Calendar.Models.CalendarItems;
using Infrastructure.Calendar.Entities;
using Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar.Repositories;

internal class CalendarItemRepository
    : BaseRepository<CalendarItem, CalendarItemEntity>,
        ICalendarItemRepository
{
    public CalendarItemRepository(DbContext context)
        : base(context) { }

    protected override CalendarItem MapToDomain(CalendarItemEntity entity)
    {
        throw new NotImplementedException();
    }

    protected override CalendarItemEntity MapToEntity(CalendarItem domain)
    {
        throw new NotImplementedException();
    }
}
