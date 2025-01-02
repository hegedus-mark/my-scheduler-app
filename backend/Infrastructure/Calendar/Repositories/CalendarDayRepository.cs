using Calendar.Application.Contracts.Repositories;
using Calendar.Application.DataTransfer.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

internal class CalendarDayRepository : BaseRepository<CalendarDayDto>, ICalendarDayRepository
{
    public CalendarDayRepository(DbContext context)
        : base(context) { }
}
