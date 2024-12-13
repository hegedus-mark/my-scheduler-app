using Scheduler.Core.Enum;

namespace Scheduler.Core.Models;

public class RecurrencePattern : EntityBase
{
    public RecurrenceType Type { get; set; }
    public int Interval { get; set; } // Every X days/weeks/months
    public DateTime? EndDate { get; set; }

    // For weekly recurrences
    public DaysOfWeek SelectedDays { get; set; }
}
