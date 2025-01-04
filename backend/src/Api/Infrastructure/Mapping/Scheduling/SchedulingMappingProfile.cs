using Api.Models.Scheduling.Requests;
using Application.Scheduling.DataTransfer.DTOs;
using Application.Scheduling.DataTransfer.DTOs.Enums;
using Application.Scheduling.Operations.Commands;
using AutoMapper;

namespace Api.Infrastructure.Mapping.Scheduling;

public class SchedulingMappingProfile : Profile
{
    public SchedulingMappingProfile()
    {
        CreateMap<ScheduleTasksRequest, ScheduleTasksCommand>();

        CreateMap<TaskRequest, TaskItemDto>()
            .ForMember(
                dest => dest.TaskItemStatus,
                opt => opt.MapFrom(_ => TaskItemStatusDto.Draft)
            );
    }
}
