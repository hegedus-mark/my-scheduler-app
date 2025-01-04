using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.Operations.Commands;
using SharedKernel.Common.Results;

namespace Application.Scheduling.Operations.Interfaces;

public interface ISchedulingService
{
    Task<Result<SchedulingResultDto>> ScheduleTaskAsync(ScheduleTasksCommand command);
}
