using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using SharedKernel.Results;

namespace Application.Scheduling.Interfaces;

public interface ISchedulingService
{
    Task<Result<SchedulingResultDto>> ScheduleTaskAsync(ScheduleTasksCommand command);
}
