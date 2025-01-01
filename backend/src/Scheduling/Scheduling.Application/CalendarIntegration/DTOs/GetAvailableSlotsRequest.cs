using SharedKernel.Domain.ValueObjects;

namespace Scheduling.Application.CalendarIntegration.DTOs;

public class GetAvailableSlotsRequest
{
    public DateRange RequestedWindow { get; init; }
}
