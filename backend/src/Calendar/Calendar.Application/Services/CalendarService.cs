using Calendar.Application.Commands;
using Calendar.Application.Interfaces;
using Calendar.Application.Queries;
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
