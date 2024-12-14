using Scheduler.Domain.Models.Base;
using Scheduler.Domain.Shared.Enums;

namespace Scheduler.Domain.Models;

public class RecurrencePattern : EntityBase
{
    public RecurrenceType Type { get; set; }
    public int Interval { get; set; } // Every X days/weeks/months
    public DateTime? EndDate { get; set; }

    // For weekly recurrences
    public DaysOfWeek SelectedDays { get; set; }
}
