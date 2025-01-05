using Application.Calendar.Commands;
using Application.Calendar.Queries;
using Domain.Shared.ValueObjects;
using SharedKernel.Results;

namespace Application.Calendar.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command);
}
