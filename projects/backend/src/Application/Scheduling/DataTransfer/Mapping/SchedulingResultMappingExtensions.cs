using Application.Scheduling.DataTransfer.DTOs;
using Domain.Scheduling.Results;

namespace Application.Scheduling.DataTransfer.Mapping;

public static class SchedulingResultMappingExtensions
{
    public static SchedulingResultDto ToDto(this SchedulingResult schedulingResult)
    {
        return new SchedulingResultDto
        {
            FailedTasks = schedulingResult.FailedTasks.Select(t => t.ToDto()).ToList(),
            ScheduledTasks = schedulingResult.ScheduledTasks.Select(t => t.ToDto()).ToList(),
            HasFailedTasks = schedulingResult.HasFailedTasks,
        };
    }

    public static SchedulingResult ToDomain(this SchedulingResultDto dto)
    {
        return SchedulingResult.Create(
            dto.ScheduledTasks.Select(t => t.ToDomain()),
            dto.FailedTasks.Select(t => t.ToDomain())
        );
    }
}
