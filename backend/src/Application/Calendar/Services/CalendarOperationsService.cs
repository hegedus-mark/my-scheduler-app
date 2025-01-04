using Application.Calendar.Operations.Interfaces;
using Application.Scheduling.CalendarIntegration;
using Application.Scheduling.CalendarIntegration.DTOs;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Application.Calendar.Services;

public class CalendarOperationsService : ISchedulingCalendarOperations
{
    private ICalendarService _calendarService;

    public CalendarOperationsService(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    )
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateReservations(ReserveCalendarSlotRequest request)
    {
        throw new NotImplementedException();
    }
}
