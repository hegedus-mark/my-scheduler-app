using Application.Scheduling.CalendarIntegration.DTOs;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Application.Scheduling.CalendarIntegration;

public interface ISchedulingCalendarOperations
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    );

    Task<Result> CreateReservations(ReserveCalendarSlotRequest request);
}
