using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;

namespace Application.Scheduling.Interfaces;

public interface ISchedulingService
{
    Task<SchedulingResultDto> ScheduleTaskAsync(ScheduleTasksCommand command);
}
