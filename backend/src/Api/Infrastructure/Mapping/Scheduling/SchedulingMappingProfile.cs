using Api.Models.Scheduling.Requests;
using Application.Scheduling.Commands;
using Application.Scheduling.DataTransfer.DTOs;
using AutoMapper;
using Domain.Scheduling.Models.Enums;

namespace Api.Infrastructure.Mapping.Scheduling;

public class SchedulingMappingProfile : Profile
{
    public SchedulingMappingProfile()
    {
        CreateMap<ScheduleTasksRequest, ScheduleTasksCommand>();

        CreateMap<TaskRequest, TaskItemDto>()
            .ForMember(dest => dest.TaskItemStatus, opt => opt.MapFrom(_ => TaskItemStatus.Draft));
    }
}
