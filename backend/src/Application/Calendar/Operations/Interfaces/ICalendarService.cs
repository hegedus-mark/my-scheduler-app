using Application.Calendar.Operations.Commands;
using Application.Calendar.Operations.Queries;
using SharedKernel.Common.Results;
using SharedKernel.Domain.ValueObjects;

namespace Application.Calendar.Operations.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command);
}
