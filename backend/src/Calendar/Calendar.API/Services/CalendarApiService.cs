using AutoMapper;
using Calendar.Application.Operations.Commands;
using Calendar.Application.Operations.Interfaces;
using Calendar.Application.Operations.Queries;
using Scheduling.Application.CalendarIntegration;
using Scheduling.Application.CalendarIntegration.DTOs;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.API.Services;

public class CalendarApiService : ICalendarApi
{
    private readonly ICalendarService _calendarService;
    private readonly IMapper _mapper;

    public CalendarApiService(ICalendarService calendarService, IMapper mapper)
    {
        _calendarService = calendarService;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    )
    {
        var query = _mapper.Map<GetAvailableSlotsQuery>(request);
        return await _calendarService.GetAvailableTimeWindows(query);
    }

    public async Task<Result> CreateReservations(ReserveCalendarSlotRequest request)
    {
        var command = _mapper.Map<ReserveTaskSlotCommand>(request);
        return await _calendarService.ReserveSlotForTask(command);
    }
}
