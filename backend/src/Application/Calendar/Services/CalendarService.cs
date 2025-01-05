using Application.Calendar.Commands;
using Application.Calendar.Interfaces;
using Application.Calendar.Queries;
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
