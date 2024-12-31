using Api.Mapping.Extensions;
using Api.Models.Requests;
using AutoMapper;
using Scheduler.Application.Commands.ScheduleTasks;

namespace Api.Mapping;

public class SchedulingMappingProfile : Profile
{
    public SchedulingMappingProfile()
    {
        CreateMap<TaskRequestDto, TaskToSchedule>();
        CreateMap<ScheduleTasksRequest, ScheduleTasksCommand>()
            .ConstructUsing(
                (src, ctx) =>
                    new ScheduleTasksCommand(
                        ctx.Mapper.Map<List<TaskToSchedule>>(src.Tasks),
                        src.WindowStart.ToDateRange(src.WindowEnd)
                    )
            );
    }
}
