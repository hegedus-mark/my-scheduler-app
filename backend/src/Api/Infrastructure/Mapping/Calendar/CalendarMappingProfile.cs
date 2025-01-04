using Application.Calendar.Operations.Commands;
using Application.Calendar.Operations.Queries;
using Application.Scheduling.CalendarIntegration.DTOs;
using AutoMapper;

namespace Api.Infrastructure.Mapping.Calendar;

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
