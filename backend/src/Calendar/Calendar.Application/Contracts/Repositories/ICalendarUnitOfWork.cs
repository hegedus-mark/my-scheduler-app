using SharedKernel.Persistence;

namespace Calendar.Application.Contracts.Repositories;

public interface ICalendarUnitOfWork : IBaseUnitOfWork
{
    ICalendarDayRepository CalendarDays { get; }
    ICalendarItemRepository CalendarItems { get; }
}
