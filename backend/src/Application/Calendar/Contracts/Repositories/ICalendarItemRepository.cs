using Application.Shared.Contracts;
using Domain.Calendar.Models.CalendarItems;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarItemRepository : IBaseRepository<CalendarItem>;
