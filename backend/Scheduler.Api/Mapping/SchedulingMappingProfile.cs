using AutoMapper;
using SchedularPrototype.Mapping.Extensions;
using SchedularPrototype.Models.Requests;
using Scheduler.Application.Commands.ScheduleTasks;

namespace SchedularPrototype.Mapping;

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
