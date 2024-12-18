using Scheduler.Application.Entities;
using Scheduler.Shared.ValueObjects;

namespace Scheduler.Application.Interfaces.Infrastructure;

public interface IDayRepository : IGenericRepository<DayEntity>
{
    Task<IReadOnlyList<DayEntity>> GetDaysInRangeAsync(DateRange dateRange);
}
