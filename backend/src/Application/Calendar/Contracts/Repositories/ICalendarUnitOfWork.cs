using SharedKernel.Persistence;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarUnitOfWork : IBaseUnitOfWork
{
    ICalendarDayRepository CalendarDays { get; }
    ICalendarItemRepository CalendarItems { get; }
}
