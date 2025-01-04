using Application.Scheduling.CalendarIntegration.DTOs;
using Domain.Shared.ValueObjects;
using SharedKernel.Common.Results;

namespace Application.Scheduling.CalendarIntegration;

public interface ISchedulingCalendarOperations
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    );

    Task<Result> CreateReservations(ReserveCalendarSlotRequest request);
}
