using Application.Shared.Contracts;
using Domain.Calendar.Models.CalendarDays;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarDayRepository : IBaseRepository<CalendarDay>;
