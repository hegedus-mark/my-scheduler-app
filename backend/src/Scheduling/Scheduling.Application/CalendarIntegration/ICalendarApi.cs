using Scheduling.Application.CalendarIntegration.DTOs;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Scheduling.Application.CalendarIntegration;

public interface ICalendarApi
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsRequest request
    );

    Task<Result> CreateReservations(ReserveCalendarSlotRequest request);
}
