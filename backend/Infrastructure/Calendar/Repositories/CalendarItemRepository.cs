using Calendar.Application.Contracts.Repositories;
using Calendar.Application.DataTransfer.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

internal class CalendarItemRepository : BaseRepository<CalendarItemDto>, ICalendarItemRepository
{
    public CalendarItemRepository(DbContext context)
        : base(context) { }
}
