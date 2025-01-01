using SharedKernel.Domain.Enums;

namespace Calendar.Application.DTOs;

public class RecurrencePatternDto
{
    public Guid Id { get; set; }
    public string RecurrenceType { get; set; }
    public int Interval { get; set; }
    public DateTime? EndDate { get; set; }
    public DaysOfWeek? SelectedDays { get; set; }
}
