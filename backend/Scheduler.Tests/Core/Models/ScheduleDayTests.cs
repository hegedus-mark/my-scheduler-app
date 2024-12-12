using Moq;
using Scheduler.Core.Enum;
using Scheduler.Core.Models;
using Scheduler.Core.Models.CalendarItems;
using Scheduler.Core.Models.Scoring;

namespace Tests.Core.Models;

public class ScheduleDayTests
{
    [Fact]
    public void Constructor_InitializesWithCorrectState()
    {
        // Arrange
        var setup = new TestSetup();

        // Assert
        Assert.Equal(setup.TestDate, setup.Day.DayDate);
        Assert.Equal(setup.WorkingHours, setup.Day.WorkingHours);
        Assert.Empty(setup.Day.CalendarItems);
        Assert.Single(setup.Day.FreeSlots);
        Assert.Equal(setup.WorkingHours, setup.Day.FreeSlots[0]);
    }

    [Fact]
    public void AddScheduledTask_WithValidTimeSlot_UpdatesFreeSlots()
    {
        // Arrange
        var setup = new TestSetup();
        var task = setup.CreateTask();
        var timeSlot = TimeSlot.Create(new TimeOnly(10, 0), new TimeOnly(11, 0));

        // Act
        setup.Day.AddScheduledTask(task, timeSlot);

        // Assert
        Assert.Equal(2, setup.Day.FreeSlots.Count);

        // Verify the free slots are correctly split
        var firstFreeSlot = setup.Day.FreeSlots[0];
        var secondFreeSlot = setup.Day.FreeSlots[1];

        Assert.Equal(setup.WorkingHours.Start, firstFreeSlot.Start);
        Assert.Equal(timeSlot.Start, firstFreeSlot.End);
        Assert.Equal(timeSlot.End, secondFreeSlot.Start);
        Assert.Equal(setup.WorkingHours.End, secondFreeSlot.End);
    }

    [Fact]
    public void AddScheduledTask_AfterDueDate_ThrowsInvalidOperationException()
    {
        // Arrange
        var setup = new TestSetup();
        var dueDate = setup.TestDate.ToDateTime(new TimeOnly(12, 0));
        var task = setup.CreateTask(dueDate: dueDate);
        var lateTimeSlot = TimeSlot.Create(new TimeOnly(13, 0), new TimeOnly(14, 0));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => setup.Day.AddScheduledTask(task, lateTimeSlot)
        );
        Assert.Contains("Scheduled date is after task due date", exception.Message);
    }

    [Fact]
    public void AddScheduledTask_OutsideWorkingHours_ThrowsInvalidOperationException()
    {
        // Arrange
        var setup = new TestSetup(workStart: new TimeOnly(9, 0), workEnd: new TimeOnly(17, 0));

        var task = setup.CreateTask();
        var earlyTimeSlot = TimeSlot.Create(new TimeOnly(8, 0), new TimeOnly(9, 0));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => setup.Day.AddScheduledTask(task, earlyTimeSlot)
        );
        Assert.Contains("Time slot must be within working hours", exception.Message);
    }

    [Fact]
    public void AddScheduledTask_OverlappingExistingTask_ThrowsInvalidOperationException()
    {
        // Arrange
        var setup = new TestSetup();

        // Add first task
        var firstTask = setup.CreateTask();
        var firstTimeSlot = TimeSlot.Create(new TimeOnly(10, 0), new TimeOnly(11, 0));
        setup.Day.AddScheduledTask(firstTask, firstTimeSlot);

        // Try to add overlapping task
        var secondTask = setup.CreateTask();
        var overlappingTimeSlot = TimeSlot.Create(new TimeOnly(10, 30), new TimeOnly(11, 30));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => setup.Day.AddScheduledTask(secondTask, overlappingTimeSlot)
        );
        Assert.Contains("Time slot overlaps with an existing calendar item", exception.Message);
    }

    [Fact]
    public void GetEvents_ReturnsOnlyEvents()
    {
        // Arrange
        var setup = new TestSetup();

        // Add a mix of events and tasks
        var task = setup.CreateTask();
        var taskSlot = TimeSlot.Create(new TimeOnly(10, 0), new TimeOnly(11, 0));
        setup.Day.AddScheduledTask(task, taskSlot);

        var eventSlot = TimeSlot.Create(new TimeOnly(14, 0), new TimeOnly(15, 0));
        var eventItem = setup.CreateEvent(eventSlot, new RecurrencePattern());
        setup.Day.AddEvent(eventItem);

        // Act
        var events = setup.Day.GetEvents();

        // Assert
        Assert.Single(events);
        Assert.IsType<Event>(events.First());
        Assert.Equal(eventItem, events.First());
    }

    [Fact]
    public void GetTasks_ReturnsOnlyTasks()
    {
        // Arrange
        var setup = new TestSetup();

        // Add a mix of events and tasks
        var task = setup.CreateTask();
        var taskSlot = TimeSlot.Create(new TimeOnly(10, 0), new TimeOnly(11, 0));
        var scheduledTask = setup.Day.AddScheduledTask(task, taskSlot);

        var eventSlot = TimeSlot.Create(new TimeOnly(14, 0), new TimeOnly(15, 0));
        var eventItem = setup.CreateEvent(eventSlot, new RecurrencePattern());
        setup.Day.AddEvent(eventItem);

        // Act
        var tasks = setup.Day.GetTasks();

        // Assert
        Assert.Single(tasks);
        Assert.IsType<ScheduledTask>(tasks.First());
        Assert.Equal(scheduledTask, tasks.First());
    }

    [Fact]
    public void AddScheduledTask_MultipleValidTasks_MaintainsCorrectFreeSlots()
    {
        // Arrange
        var setup = new TestSetup();

        // Add three tasks with gaps between them
        var slots = new[]
        {
            TimeSlot.Create(new TimeOnly(10, 0), new TimeOnly(11, 0)),
            TimeSlot.Create(new TimeOnly(12, 0), new TimeOnly(13, 0)),
            TimeSlot.Create(new TimeOnly(14, 0), new TimeOnly(15, 0)),
        };

        // Act
        foreach (var slot in slots)
        {
            var task = setup.CreateTask();
            setup.Day.AddScheduledTask(task, slot);
        }

        // Assert
        Assert.Equal(4, setup.Day.FreeSlots.Count);
        Assert.Equal(3, setup.Day.CalendarItems.Count);

        // Verify the order and continuity of free slots
        var freeSlots = setup.Day.FreeSlots.OrderBy(s => s.Start).ToList();
        Assert.Equal(setup.WorkingHours.Start, freeSlots[0].Start);
        Assert.Equal(slots[0].Start, freeSlots[0].End);

        Assert.Equal(slots[0].End, freeSlots[1].Start);
        Assert.Equal(slots[1].Start, freeSlots[1].End);

        Assert.Equal(slots[1].End, freeSlots[2].Start);
        Assert.Equal(slots[2].Start, freeSlots[2].End);

        Assert.Equal(slots[2].End, freeSlots[3].Start);
        Assert.Equal(setup.WorkingHours.End, freeSlots[3].End);
    }

    // Helper class to reduce test setup boilerplate
    private class TestSetup
    {
        public TestSetup(
            DateOnly? date = null,
            TimeOnly? workStart = null,
            TimeOnly? workEnd = null
        )
        {
            TestDate = date ?? DateOnly.FromDateTime(DateTime.Today);
            workStart ??= new TimeOnly(9, 0);
            workEnd ??= new TimeOnly(17, 0);

            WorkingHours = TimeSlot.Create(workStart.Value, workEnd.Value);
            Day = new ScheduleDay(TestDate, WorkingHours);
        }

        public DateOnly TestDate { get; }
        public TimeSlot WorkingHours { get; }
        public ScheduleDay Day { get; }

        public TaskItem CreateTask(
            string name = "Test Task",
            DateTime? dueDate = null,
            TimeSpan? duration = null,
            PriorityLevel priority = PriorityLevel.Medium
        )
        {
            dueDate ??= TestDate.ToDateTime(new TimeOnly(23, 59, 59));
            duration ??= TimeSpan.FromHours(1);

            var mockScoring = new Mock<IScoringStrategy>();
            mockScoring.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(0);

            return new TaskItem(name, dueDate.Value, priority, mockScoring.Object, duration.Value);
        }

        public Event CreateEvent(TimeSlot timeSlot, RecurrencePattern recurrencePattern)
        {
            return new Event(timeSlot, recurrencePattern);
        }
    }
}
