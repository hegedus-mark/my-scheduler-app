using Calendar.Application.DataTransfer.DTOs;
using SharedKernel.Persistence;

namespace Calendar.Application.Contracts.Repositories;

public interface ICalendarItemRepository : IBaseRepository<CalendarItemDto>;
