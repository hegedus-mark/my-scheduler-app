using Domain.Calendar.Models.Enums;

namespace Domain.Calendar.ValueObjects;

public readonly record struct RecurrencePattern
{
    // Private constructor ensures validation through factory methods
    private RecurrencePattern(
        RecurrenceType type,
        int interval,
        DateTime? endDate,
        DaysOfWeek? selectedDays
    )
    {
        // We can still have all our validation logic
        if (interval <= 0)
            throw new ArgumentException("Interval must be positive", nameof(interval));

        if (type == RecurrenceType.Weekly && !selectedDays.HasValue)
            throw new ArgumentException(
                "Weekly recurrence must specify days",
                nameof(selectedDays)
            );

        Type = type;
        Interval = interval;
        EndDate = endDate;
        SelectedDays = selectedDays;
    }

    public RecurrenceType Type { get; }
    public int Interval { get; }
    public DateTime? EndDate { get; }
    public DaysOfWeek? SelectedDays { get; }

    public static RecurrencePattern CreateDaily(int interval, DateTime? endDate = null)
    {
        return new RecurrencePattern(RecurrenceType.Daily, interval, endDate, null);
    }

    public static RecurrencePattern CreateWeekly(
        int interval,
        DaysOfWeek selectedDays,
        DateTime? endDate = null
    )
    {
        return new RecurrencePattern(RecurrenceType.Weekly, interval, endDate, selectedDays);
    }

    public bool IsActive(DateTime date)
    {
        if (EndDate.HasValue && date > EndDate.Value)
            return false;

        return Type switch
        {
            RecurrenceType.Daily => true,
            RecurrenceType.Weekly => SelectedDays!.Value.HasFlag(
                (DaysOfWeek)(1 << (int)date.DayOfWeek)
            ),
            _ => throw new NotImplementedException($"Recurrence type {Type} not implemented"),
        };
    }

    public override string ToString()
    {
        return Type switch
        {
            RecurrenceType.Daily => $"Every {Interval} day(s)",
            RecurrenceType.Weekly => $"Every {Interval} week(s) on {SelectedDays}",
            _ => base.ToString()!,
        };
    }
}
