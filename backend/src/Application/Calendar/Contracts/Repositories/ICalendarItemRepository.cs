using Calendar.Domain.Models.CalendarItems;
using SharedKernel.Persistence;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarItemRepository : IBaseRepository<CalendarItem>;
