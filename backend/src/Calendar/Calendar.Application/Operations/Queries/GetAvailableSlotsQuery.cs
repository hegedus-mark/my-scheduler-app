using SharedKernel.Domain.ValueObjects;

namespace Calendar.Application.Operations.Queries;

public class GetAvailableSlotsQuery
{
    public DateRange RequestedWindow { get; init; }
}
