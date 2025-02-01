using Application.Calendar.Interfaces.Repositories;
using Application.Scheduling.Interfaces.Repositories;
using Infrastructure.Calendar.Repositories;
using Infrastructure.Scheduling.Repositories;
using Infrastructure.Shared.Context;

namespace Infrastructure.Shared.Persistence;

/// <summary>
///     A shared implementation of the Unit of Work pattern that combines calendar and scheduling repositories
///     within a single transaction boundary. Implements lazy loading of repositories to improve performance.
/// </summary>
/// <remarks>
///     <para>
///         This implementation:
///         - Combines multiple domain-specific unit of work interfaces
///         - Implements lazy loading of repositories
///         - Ensures repositories share the same database context
///         - Maintains a single transaction scope across all operations
///     </para>
/// </remarks>
public class SharedUnitOfWork : BaseUnitOfWork, ICalendarUnitOfWork, ISchedulingUnitOfWork
{
    /// <summary>
    ///     Cache for dynamically created repositories.
    /// </summary>
    /// <remarks>
    ///     This dictionary can be used to implement additional repository types
    ///     without modifying the class structure.
    /// </remarks>
    private readonly Dictionary<Type, object> _repositories = new();

    /// <summary>
    ///     Backing field for the CalendarDay repository. Initialized on first access.
    /// </summary>
    private ICalendarDayRepository? _calendarDayRepository;

    /// <summary>
    ///     Backing field for the CalendarItem repository. Initialized on first access.
    /// </summary>
    private ICalendarItemRepository? _calendarItemRepository;

    /// <summary>
    ///     Backing field for the TaskItem repository. Initialized on first access.
    /// </summary>
    private ITaskItemRepository? _taskRepository;

    /// <summary>
    ///     Initializes a new instance of the SharedUnitOfWork class.
    /// </summary>
    /// <param name="context">The database context to use for all repositories.</param>
    public SharedUnitOfWork(AppDbContext context)
        : base(context) { }

    /// <summary>
    ///     Gets the CalendarDay repository, creating it if it doesn't exist.
    /// </summary>
    /// <remarks>
    ///     The repository is lazy-loaded on first access to improve performance
    ///     when not all repositories are needed.
    /// </remarks>
    public ICalendarDayRepository CalendarDays =>
        _calendarDayRepository ??= new CalendarDayRepository(Context);

    /// <summary>
    ///     Gets the CalendarItem repository, creating it if it doesn't exist.
    /// </summary>
    /// <remarks>
    ///     The repository is lazy-loaded on first access to improve performance
    ///     when not all repositories are needed.
    /// </remarks>
    public ICalendarItemRepository CalendarItems =>
        _calendarItemRepository ??= new CalendarItemRepository(Context);

    /// <summary>
    ///     Gets the TaskItem repository, creating it if it doesn't exist.
    /// </summary>
    /// <remarks>
    ///     The repository is lazy-loaded on first access to improve performance
    ///     when not all repositories are needed.
    /// </remarks>
    public ITaskItemRepository TaskItems => _taskRepository ??= new TaskItemRepository(Context);
}
