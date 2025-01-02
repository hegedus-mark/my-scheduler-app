using Calendar.Application.Operations.Commands;
using Calendar.Application.Operations.Queries;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Calendar.Application.Operations.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command);
}
