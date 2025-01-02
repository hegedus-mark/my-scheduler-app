namespace Calendar.Application.Contracts.Repositories;

public interface ICalendarRepositoryContainer
{
    ICalendarDayRepository CalendarDays { get; }
    ICalendarItemRepository CalendarItems { get; }
}
