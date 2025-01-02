using Calendar.Application.Operations.Commands;
using Calendar.Application.Operations.Interfaces;
using Calendar.Application.Operations.Queries;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Application.Services;

public class CalendarService : ICalendarService
{
    public Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(
        GetAvailableSlotsQuery query
    )
    {
        throw new NotImplementedException();
    }
}
