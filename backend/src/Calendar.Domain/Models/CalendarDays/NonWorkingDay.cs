using Calendar.Domain.Models.CalendarItems;

namespace Calendar.Domain.Models.CalendarDays;

public class NonWorkingDay : CalendarDay
{
    private readonly List<CalendarItem> _calendarItems;

    private NonWorkingDay(Guid? id, DateOnly dayDate)
        : base(dayDate, id)
    {
        _calendarItems = new List<CalendarItem>();
    }

    public override bool IsWorkingDay => false;

    public static NonWorkingDay Create(DateOnly dayDate)
    {
        return new NonWorkingDay(null, dayDate);
    }

    public static NonWorkingDay Load(Guid id, DateOnly dayDate)
    {
        return new NonWorkingDay(id, dayDate);
    }
}
