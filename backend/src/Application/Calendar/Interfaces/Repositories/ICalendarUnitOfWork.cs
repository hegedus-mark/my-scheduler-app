using Application.Shared.Contracts;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarUnitOfWork : IBaseUnitOfWork
{
    ICalendarDayRepository CalendarDays { get; }
    ICalendarItemRepository CalendarItems { get; }
}
