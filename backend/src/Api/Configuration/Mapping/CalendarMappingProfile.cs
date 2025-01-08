using Application.Calendar.Commands;
using Application.Calendar.Queries;
using Application.Scheduling.CalendarIntegration.DTOs;
using AutoMapper;

namespace Api.Configuration.Mapping;

public class CalendarMappingProfile : Profile
{
    public CalendarMappingProfile()
    {
        CreateMap<ReserveCalendarSlotRequest, ReserveTaskSlotCommand>()
            .ConstructUsing(
                (src, _) =>
                    new ReserveTaskSlotCommand(src.Title, src.TaskId, src.StartTime, src.EndTime)
            );

        CreateMap<GetAvailableSlotsRequest, GetAvailableSlotsQuery>();
    }
}
