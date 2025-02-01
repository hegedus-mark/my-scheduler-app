using Application.Calendar.Commands;
using Application.Calendar.Interfaces;
using Application.Calendar.Queries;
using Domain.Shared.ValueObjects;

namespace Application.Calendar.Services;

public class CalendarService : ICalendarService
{
    public Task ReserveSlotForTask(ReserveTaskSlotCommand command)
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
