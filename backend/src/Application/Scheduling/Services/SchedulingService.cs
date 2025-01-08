using Application.Scheduling.CalendarIntegration;
using Application.Scheduling.CalendarIntegration.DTOs;
using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.Mapping;
using Application.Scheduling.Interfaces;
using Application.Scheduling.Interfaces.Repositories;
using Domain.Scheduling.Services;

namespace Application.Scheduling.Services;

//TODO: Finish this implementation, we have to implement two-phase scheduling (plan and finalize)
public class SchedulingService : ISchedulingService
{
    private readonly ISchedulingCalendarOperations _calendarApi;
    private readonly ISchedulingStrategy _schedulingStrategy;
    private ISchedulingUnitOfWork _unitOfWork;

    public SchedulingService(
        ISchedulingUnitOfWork unitOfWork,
        ISchedulingCalendarOperations calendarApi,
        ISchedulingStrategy schedulingStrategy
    )
    {
        _unitOfWork = unitOfWork;
        _calendarApi = calendarApi;
        _schedulingStrategy = schedulingStrategy;
    }

    public async Task<SchedulingResultDto> ScheduleTaskAsync(ScheduleTasksCommand command)
    {
        throw new NotImplementedException();
        var window = GetWindow(
            command.WindowStart,
            command.WindowEnd,
            GetLastTaskDueDate(command.TaskItems)
        );
        var tasksToBeScheduled = command.TaskItems.Select(t => t.ToDomain()).ToList();

        var timeWindows = await _calendarApi.GetAvailableTimeWindows(window);

        var result = _schedulingStrategy.Schedule(tasksToBeScheduled, timeWindows);
    }

    private GetAvailableSlotsRequest GetWindow(
        DateTime? windowStart,
        DateTime? windowEnd,
        DateTime lastDueDate
    )
    {
        var start = windowStart.HasValue ? windowStart.Value : DateTime.Today.AddDays(1);
        var end = windowEnd.HasValue ? windowEnd.Value : lastDueDate;

        return new GetAvailableSlotsRequest { WindowStart = start, WindowEnd = end };
    }

    private DateTime GetLastTaskDueDate(IReadOnlyCollection<TaskItemDto> tasks)
    {
        return tasks.Max(t => t.DueDate);
    }
}
