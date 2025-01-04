using Application.Calendar.Contracts.Repositories;
using Application.Scheduling.Contracts.Repositories;
using Infrastructure.Calendar.Repositories;
using Infrastructure.Context;
using Infrastructure.Scheduling.Repositories;

namespace Infrastructure.Persistence;

public class SharedUnitOfWork : BaseUnitOfWork, ICalendarUnitOfWork, ISchedulingUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
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
