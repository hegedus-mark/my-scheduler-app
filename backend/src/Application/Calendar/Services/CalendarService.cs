using Application.Calendar.Operations.Commands;
using Application.Calendar.Operations.Interfaces;
using Application.Calendar.Operations.Queries;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Application.Calendar.Services;

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
