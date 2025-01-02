using Calendar.Application.Contracts.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Persistence;

public class SharedUnitOfWork : BaseUnitOfWork, ICalendarRepositoryContainer
{
    private ICalendarDayRepository? _calendarDayRepository;
    private ICalendarItemRepository? _calendarItemRepository;

    public SharedUnitOfWork(AppDbContext context)
        : base(context) { }

    public ICalendarDayRepository CalendarDays =>
        _calendarDayRepository ??= new CalendarDayRepository(Context);

    public ICalendarItemRepository CalendarItems =>
        _calendarItemRepository ??= new CalendarItemRepository(Context);
}
