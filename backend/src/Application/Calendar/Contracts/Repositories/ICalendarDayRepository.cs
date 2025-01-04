using Domain.Calendar.Models.CalendarDays;
using SharedKernel.Persistence;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarDayRepository : IBaseRepository<CalendarDay>;
