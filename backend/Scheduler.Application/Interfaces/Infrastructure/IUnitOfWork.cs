namespace Scheduler.Application.Interfaces.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IDayRepository CalendarDays { get; }
    Task<int> SaveChangesAsync();
}
