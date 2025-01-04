using Application.Calendar.Operations.Commands;
using Application.Calendar.Operations.Interfaces;
using Application.Calendar.Operations.Queries;
using Domain.Shared.ValueObjects;
using SharedKernel.Results;

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
