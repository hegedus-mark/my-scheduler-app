using Application.Calendar.Contracts.Repositories;
using Application.Calendar.DataTransfer.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar.Repositories;

internal class CalendarItemRepository : BaseRepository<CalendarItemDto>, ICalendarItemRepository
{
    public CalendarItemRepository(DbContext context)
        : base(context) { }
}
