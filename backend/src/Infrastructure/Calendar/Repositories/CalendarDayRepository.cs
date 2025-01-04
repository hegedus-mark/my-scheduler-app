using Application.Calendar.Contracts.Repositories;
using Application.Calendar.DataTransfer.DTOs;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar.Repositories;

internal class CalendarDayRepository : BaseRepository<CalendarDayDto>, ICalendarDayRepository
{
    public CalendarDayRepository(DbContext context)
        : base(context) { }
}
