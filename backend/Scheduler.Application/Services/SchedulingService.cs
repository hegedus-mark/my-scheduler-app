using Scheduler.Application.Commands.ScheduleTasks;
using Scheduler.Application.Entities;
using Scheduler.Application.Interfaces.Infrastructure;
using Scheduler.Application.Interfaces.Mapping;
using Scheduler.Application.Interfaces.Services;
using Scheduler.Domain.Interfaces;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Models.Results;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared.Enums;
using Scheduler.Shared.Extensions;
using Scheduler.Shared.Models;
using Scheduler.Shared.ValueObjects;

namespace Scheduler.Application.Services;

public class SchedulingService : ISchedulingService
{
    private const int DEFAULT_SCHEDULING_WINDOW = 30; //TODO: Put this in appsettings;
    private readonly IMapper<ICalendarDay, DayEntity> _dayMapper;
    private readonly IScoringStrategy _scoringStrategy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserScheduleConfig _userScheduleConfig;

    public SchedulingService(
        IUnitOfWork unitOfWork,
        IScoringStrategy scoringStrategy,
        UserScheduleConfig userScheduleConfig,
        IMapper<ICalendarDay, DayEntity> dayMapper
    )
    {
        _unitOfWork = unitOfWork;
        _scoringStrategy = scoringStrategy;
        _userScheduleConfig = userScheduleConfig;
        _dayMapper = dayMapper;
    }

    public async Task<Result<SchedulingResult>> ScheduleTasksAsync(ScheduleTasksCommand command)
    {
        // Validate input
        if (command.Tasks == null || !command.Tasks.Any())
            return Result<SchedulingResult>.Failure(
                Error.Validation("No tasks provided for scheduling")
            );

        // Convert command tasks to domain tasks
        var tasksToSchedule = command
            .Tasks.Select(t => new TaskItem(
                t.Name,
                t.DueDate,
                t.Priority,
                _scoringStrategy,
                t.Duration
            ))
            .ToList();

        // Get existing Days From Repository
        var schedulingWindow =
            command.SchedulingWindow
            ?? DateRange.CreateFromDuration(
                DateTime.Now.ToDateOnly(),
                0,
                DEFAULT_SCHEDULING_WINDOW
            );
        var existingDays = await _unitOfWork.CalendarDays.GetDaysInRangeAsync(schedulingWindow);

        //Map Days
        var mappedDays = GetOrCreateCalendarDays(
            schedulingWindow,
            _userScheduleConfig,
            existingDays
        );

        // Schedule tasks
        var scheduleManager = new ScheduleWindow(mappedDays, new PrioritizedSchedulingStrategy());
        var result = scheduleManager.ScheduleTasks(tasksToSchedule);

        // Persist changes
        await PersistNewDaysAsync(mappedDays, existingDays);

        return Result<SchedulingResult>.Success(result);
    }

    private List<ICalendarDay> GetOrCreateCalendarDays(
        DateRange dateRange,
        UserScheduleConfig userConfig,
        IReadOnlyList<DayEntity> existingDays
    )
    {
        var daysDict = existingDays.ToDictionary(
            d => d.Date.ToDateOnly(),
            d => _dayMapper.ToDomain(d)
        );

        var calendarDays = new List<ICalendarDay>();

        foreach (var date in dateRange.GetDates())
        {
            if (daysDict.TryGetValue(date, out var existingDay))
            {
                calendarDays.Add(existingDay);
                continue;
            }

            var newDay = CreateNewCalendarDay(date, userConfig);
            calendarDays.Add(newDay);
        }

        return calendarDays;
    }

    private async Task PersistNewDaysAsync(
        List<ICalendarDay> calendarDays,
        IReadOnlyList<DayEntity> existingDays
    )
    {
        var newDays = calendarDays
            .Where(d => existingDays.All(e => e.Id != d.Id))
            .Select(d => _dayMapper.ToEntity(d));

        await _unitOfWork.CalendarDays.AddRangeAsync(newDays);
        await _unitOfWork.SaveChangesAsync();
    }

    private ICalendarDay CreateNewCalendarDay(DateOnly date, UserScheduleConfig userConfig)
    {
        var isWorkingDay = IsWorkingDay(date, userConfig.WorkingDays);

        if (!isWorkingDay)
            return NonWorkingDay.Create(date);

        var workingHours = TimeSlot.Create(
            userConfig.DefaultWorkStartTime,
            userConfig.DefaultWorkEndTime
        );

        return WorkingDay.Create(date, workingHours);
    }

    private bool IsWorkingDay(DateOnly date, DaysOfWeek workingDays)
    {
        var dayOfWeek = (DaysOfWeek)(1 << (int)date.DayOfWeek);
        return workingDays.HasFlag(dayOfWeek);
    }
}
