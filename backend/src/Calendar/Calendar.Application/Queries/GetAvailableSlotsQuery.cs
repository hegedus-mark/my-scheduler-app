using SharedKernel.Domain.ValueObjects;

namespace Calendar.Application.Queries;

public class GetAvailableSlotsQuery
{
    public DateRange RequestedWindow { get; init; }
}
