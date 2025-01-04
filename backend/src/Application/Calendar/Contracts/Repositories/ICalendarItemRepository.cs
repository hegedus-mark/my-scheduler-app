using Domain.Calendar.Models.CalendarItems;
using SharedKernel.Persistence;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarItemRepository : IBaseRepository<CalendarItem>;
