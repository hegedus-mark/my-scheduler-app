namespace Scheduler.Core.Models;

public partial class Day
{
    private static readonly TimeSpan MinTaskTime = new TimeSpan(0, 30, 0);
    private static readonly TimeSpan MinEventTime = new TimeSpan(0, 1, 0);

    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime DayDate { get; set; }

    public DateTime StartWork;
    public DateTime EndWork;

    private readonly List<CalendarItem> _calendarItems;
    public List<TimeSlot> FreeSlots { get; }

    public Day(DateTime dayDate, DateTime startWork, DateTime endWork)
    {
        DayDate = dayDate.Date; // Set DayDate to midnight

        if (startWork.Date != dayDate.Date || endWork.Date != dayDate.Date)
        {
            throw new ArgumentException("StartWork and EndWork must be on the same day as DayDate.");
        }

        if (startWork >= endWork)
        {
            throw new ArgumentException("StartWork must be before EndWork."); 
        }
        
        StartWork = startWork;
        EndWork = endWork;
        _calendarItems = new List<CalendarItem>();
        FreeSlots = [new TimeSlot(StartWork, EndWork)]; // At first the entire day is free
    }

    public void AddCalendarItem(CalendarItem calendarItem)
    {
        _calendarItems.Add(calendarItem);
        ReCalculateFreeSlots(calendarItem);
    }

    public IEnumerable<Event> GetEvents()
    {
        return _calendarItems.OfType<Event>().ToList();
    }

    public IEnumerable<UnscheduledTask> GetTasks()
    {
        return _calendarItems.OfType<UnscheduledTask>().ToList();
    }

    private void ReCalculateFreeSlots(CalendarItem item)
    {
        var placedTimeSlot = item.TimeSlot;
        var parentFreeSlot = FreeSlots.FirstOrDefault(placedTimeSlot.IsInsideTimeSlot);

        if (parentFreeSlot == null)
        {
            throw new InvalidOperationException(
                "Added calendarItem is not placed in a free slot, there must be an overlap");
        }

        FreeSlots.Remove(parentFreeSlot);

        if (parentFreeSlot.Start < placedTimeSlot.Start)
        {
            var firstHalfSlot = new TimeSlot(parentFreeSlot.Start, placedTimeSlot.Start);
            FreeSlots.Add(firstHalfSlot);
        }

        if (parentFreeSlot.End > placedTimeSlot.End)
        {
            var secondHalfSlot = new TimeSlot(placedTimeSlot.End, parentFreeSlot.End);
            FreeSlots.Add(secondHalfSlot);
        }
    }
}