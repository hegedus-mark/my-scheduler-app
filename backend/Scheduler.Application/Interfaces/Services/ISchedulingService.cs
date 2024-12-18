using Scheduler.Application.Commands.ScheduleTasks;
using Scheduler.Domain.Models.Results;
using Scheduler.Shared.Models;

namespace Scheduler.Application.Interfaces.Services;

public interface ISchedulingService
{
    Task<Result<SchedulingResult>> ScheduleTasksAsync(ScheduleTasksCommand command);
}
