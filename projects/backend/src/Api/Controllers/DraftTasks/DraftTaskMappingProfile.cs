using Api.Controllers.DraftTasks.Model;
using Application.DraftTasks.CreateDraftTask;
using Application.DraftTasks.UpdateDraftTask;
using AutoMapper;

namespace Api.Controllers.DraftTasks;

public class DraftTaskMappingProfile : Profile
{
    public DraftTaskMappingProfile()
    {
        CreateMap<CreateDraftTaskRequest, CreateDraftTaskCommand>();
        CreateMap<UpdateDraftTaskRequest, UpdateDraftTaskCommand>()
            .ForMember(dest => dest.DraftTaskId, opt =>
                opt.MapFrom((src, dest, destMember, context) => context.Items["Id"]))
            ;
    }
}