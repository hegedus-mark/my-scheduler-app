using Application.Shared.Contracts;

namespace Application.Calendar.Interfaces.Repositories;

public interface ICalendarUnitOfWork : IBaseUnitOfWork
{
    ICalendarDayRepository CalendarDays { get; }
    ICalendarItemRepository CalendarItems { get; }
}
