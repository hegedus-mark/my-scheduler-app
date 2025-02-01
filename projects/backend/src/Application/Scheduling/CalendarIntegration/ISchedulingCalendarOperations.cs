using Application.Scheduling.CalendarIntegration.DTOs;
using Domain.Shared.ValueObjects;

namespace Application.Scheduling.CalendarIntegration;

public interface ISchedulingCalendarOperations
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    );

    Task CreateReservations(ReserveCalendarSlotRequest request);
}
