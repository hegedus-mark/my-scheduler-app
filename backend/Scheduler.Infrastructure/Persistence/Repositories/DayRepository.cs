using Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Scheduler.Application.Entities;
using Scheduler.Application.Interfaces.Infrastructure;
using Scheduler.Shared.Extensions;
using Scheduler.Shared.ValueObjects;

namespace Infrastructure.Persistence.Repositories;

internal class DayRepository : GenericRepository<DayEntity>, IDayRepository
{
    public DayRepository(DbContext context)
        : base(context) { }

    public async Task<IReadOnlyList<DayEntity>> GetDaysInRangeAsync(DateRange dateRange)
    {
        var end = dateRange.End.ToDateTime();
        var start = dateRange.Start.ToDateTime();

        return await DbSet
            .Where(c => c.Date <= end && c.Date >= start)
            .Include(d => d.CalendarItems)
            .OrderBy(d => d.Date)
            .ToListAsync();
    }
}
