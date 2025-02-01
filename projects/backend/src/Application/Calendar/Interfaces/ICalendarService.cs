using Application.Calendar.Commands;
using Application.Calendar.Queries;
using Domain.Shared.ValueObjects;

namespace Application.Calendar.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task ReserveSlotForTask(ReserveTaskSlotCommand command);
}
