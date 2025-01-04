using Application.Calendar.Operations.Commands;
using Application.Calendar.Operations.Queries;
using Domain.Shared.ValueObjects;
using SharedKernel.Common.Results;

namespace Application.Calendar.Operations.Interfaces;

public interface ICalendarService
{
    Task<IReadOnlyList<CalendarTimeWindow>> GetAvailableTimeWindows(GetAvailableSlotsQuery query);
    Task<Result> ReserveSlotForTask(ReserveTaskSlotCommand command);
}
