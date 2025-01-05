using Application.Calendar.Interfaces;
using Application.Scheduling.CalendarIntegration;
using Application.Scheduling.CalendarIntegration.DTOs;
using Domain.Shared.ValueObjects;
using SharedKernel.Results;

namespace Application.Calendar.Services;

public class CalendarOperationsService : ISchedulingCalendarOperations
{
    private ICalendarService _calendarService;

    public CalendarOperationsService(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    public Task<Result> CreateReservations(ReserveCalendarSlotRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    )
    {
        throw new NotImplementedException();
    }
}
