using Moq;
using Scheduler.Core.Enum;
using Scheduler.Core.Models;
using Scheduler.Core.Models.CalendarItems;
using Scheduler.Core.Models.Scoring;

namespace Tests.Core.Models.CalendarItems;

public class ScheduledTaskTests
{
    private TaskItem CreateSampleTask(
        string name = "Test Task",
        DateTime? dueDate = null,
        TimeSpan? duration = null,
        PriorityLevel priority = PriorityLevel.Medium
    )
    {
        // Default values that will work for most test cases
        dueDate ??= DateTime.Today.AddDays(1);
        duration ??= TimeSpan.FromHours(1);

        // We need a scoring strategy for TaskItem, but we don't want to test its logic here
        var mockScoringStrategy = new Mock<IScoringStrategy>();
        mockScoringStrategy.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(0);

        return new TaskItem(
            name,
            dueDate.Value,
            priority,
            mockScoringStrategy.Object,
            duration.Value
        );
    }

    [Fact]
    public void Constructor_WithExistingId_PreservesId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var task = CreateSampleTask();
        var timeSlot = TimeSlot.Create(new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Act
        var scheduledTask = new ScheduledTask(id, task, timeSlot);

        // Assert
        Assert.Equal(id, scheduledTask.Id);
    }

    [Theory]
    [InlineData(PriorityLevel.High)]
    [InlineData(PriorityLevel.Medium)]
    [InlineData(PriorityLevel.Low)]
    public void Properties_ReflectOriginalTaskValues(PriorityLevel priority)
    {
        // Arrange
        var name = "Test Task";
        var dueDate = DateTime.Today.AddDays(1);
        var task = CreateSampleTask(name, dueDate, priority: priority);

        var timeSlot = TimeSlot.Create(new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Act
        var scheduledTask = new ScheduledTask(task, timeSlot);

        // Assert
        Assert.Equal(name, scheduledTask.Name);
        Assert.Equal(dueDate, scheduledTask.DueDate);
        Assert.Equal(priority, scheduledTask.Priority);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesScheduledTask()
    {
        // Arrange
        var duration = TimeSpan.FromHours(1);
        var task = CreateSampleTask(duration: duration);
        var timeSlot = TimeSlot.Create(new TimeOnly(9, 0), new TimeOnly(10, 0));

        // Act
        var scheduledTask = new ScheduledTask(task, timeSlot);

        // Assert
        Assert.Equal(task, scheduledTask.OriginalTask);
        Assert.Equal(timeSlot, scheduledTask.TimeSlot);
        Assert.Equal(task.Name, scheduledTask.Name);
        Assert.Equal(task.Score, scheduledTask.Score);
        Assert.Equal(task.DueDate, scheduledTask.DueDate);
        Assert.Equal(task.PriorityLevel, scheduledTask.Priority);
    }

    [Fact]
    public void Constructor_WithMismatchedDuration_ThrowsArgumentException()
    {
        // Arrange
        var task = CreateSampleTask(duration: TimeSpan.FromHours(1));
        var timeSlot = TimeSlot.Create(new TimeOnly(9, 0), new TimeOnly(10, 30)); // 1.5 hours, doesn't match task duration

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ScheduledTask(task, timeSlot));
    }
}
