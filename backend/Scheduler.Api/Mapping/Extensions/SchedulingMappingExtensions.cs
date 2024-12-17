using Scheduler.Domain.ValueObjects;

namespace SchedularPrototype.Mapping.Extensions;

public static class SchedulingMappingExtensions
{
    public static DateRange? ToDateRange(this DateTime? start, DateTime? end)
    {
        return start.HasValue && end.HasValue
            ? new DateRange(DateOnly.FromDateTime(start.Value), DateOnly.FromDateTime(end.Value))
            : null;
    }
}
