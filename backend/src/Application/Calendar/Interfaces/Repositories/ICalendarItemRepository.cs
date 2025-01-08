using Application.Shared.Contracts;
using Domain.Calendar.Models.CalendarItems;

namespace Application.Calendar.Interfaces.Repositories;

public interface ICalendarItemRepository : IBaseRepository<CalendarItem>;
