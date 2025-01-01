using Calendar.Application.Commands;
using Calendar.Application.Queries;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Application.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command);
}
