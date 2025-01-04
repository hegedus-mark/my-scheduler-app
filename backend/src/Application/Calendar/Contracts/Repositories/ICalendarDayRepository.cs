using Application.Calendar.DataTransfer.DTOs;
using SharedKernel.Persistence;

namespace Application.Calendar.Contracts.Repositories;

public interface ICalendarDayRepository : IBaseRepository<CalendarDayDto>;
