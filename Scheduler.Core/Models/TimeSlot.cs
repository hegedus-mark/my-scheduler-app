namespace Scheduler.Core.Models;

public class TimeSlot
{
    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(1);
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public TimeSpan Duration => End - Start;

    public TimeSlot(DateTime start, DateTime end)
    {
        if (start >= end)
        {
            throw new ArgumentException("Start time must be before end time.");
        }

        if (start.Date != end.Date)
        {
            throw new ArgumentException("Start and end times must be on the same day.");
        }

        if (end - start < MinDuration) 
        {
            throw new ArgumentException("Duration must be at least 1 minute.");
        }
        
        Start = start;
        End = end;
    }

    public bool IsInsideTimeSlot(TimeSlot anotherTimeSlot)
    {
        return anotherTimeSlot.Start <= this.Start
               && anotherTimeSlot.End >= this.End;
    }
}