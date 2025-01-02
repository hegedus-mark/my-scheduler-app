using Calendar.Application.Contracts.Repositories;
using Infrastructure.Context;
using Infrastructure.Scheduling.Repositories;
using Scheduling.Application.Contracts.Repositories;

namespace Infrastructure.Persistence;

public class SharedUnitOfWork
    : BaseUnitOfWork,
        ICalendarRepositoryContainer,
        ISchedulingRepositoryContainer
{
    private ICalendarDayRepository? _calendarDayRepository;
    private ICalendarItemRepository? _calendarItemRepository;
    private ITaskItemRepository? _taskRepository;

    public SharedUnitOfWork(AppDbContext context)
        : base(context) { }

    public ICalendarDayRepository CalendarDays =>
        _calendarDayRepository ??= new CalendarDayRepository(Context);

    public ICalendarItemRepository CalendarItems =>
        _calendarItemRepository ??= new CalendarItemRepository(Context);

    public ITaskItemRepository TaskItems => _taskRepository ??= new TaskItemRepository(Context);
}
