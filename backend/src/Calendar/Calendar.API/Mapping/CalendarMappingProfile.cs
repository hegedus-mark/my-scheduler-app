using AutoMapper;
using Calendar.Application.Operations.Commands;
using Calendar.Application.Operations.Queries;
using Scheduling.Application.CalendarIntegration.DTOs;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.API.Mapping;

public class CalendarMappingProfile : Profile
{
    public CalendarMappingProfile()
    {
        CreateMap<ReserveCalendarSlotRequest, ReserveTaskSlotCommand>()
            .ConstructUsing(
                (src, ctx) =>
                    new ReserveTaskSlotCommand(src.Title, src.TaskId, src.StartTime, src.EndTime)
            );

        CreateMap<GetAvailableSlotsRequest, GetAvailableSlotsQuery>()
            .ForMember(
                dest => dest.RequestedWindow,
                opt => opt.MapFrom((src, dest) => MapDateRange(src.RequestedWindow))
            );

        CreateMap<DateRange, DateRange>().ConstructUsing((src, ctx) => MapDateRange(src));
    }

    private static DateRange MapDateRange(DateRange source)
    {
        return DateRange.CreateFromDuration(source.Start, source.Duration);
    }
}
