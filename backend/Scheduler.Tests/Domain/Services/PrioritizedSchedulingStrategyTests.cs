using FluentAssertions;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared;
using Scheduler.Domain.Shared.Enums;

namespace Tests.Domain.Services;

public class PrioritizedSchedulingStrategyTests
{
    private readonly UserScheduleConfig _defaultConfig;
    private readonly IScoringStrategy _mockScoringStrategy;
    private readonly PrioritizedSchedulingStrategy _strategy;

    public PrioritizedSchedulingStrategyTests()
    {
        _strategy = new PrioritizedSchedulingStrategy();
        _defaultConfig = UserScheduleConfig.CreateDefault();
        _mockScoringStrategy = new SimpleScoringStrategy();
    }

    [Fact]
    public void Schedule_WithNoAvailableDays_ThrowsArgumentException()
    {
        // Arrange
        var tasks = new List<TaskItem> { CreateTestTask("Task1") };

        // Act
        var action = () => _strategy.Schedule(new List<WorkingDay>(), tasks);

        // Assert
        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("Must provide at least one day for scheduling");
    }

    [Fact]
    public void Schedule_WithNoTasks_ThrowsArgumentException()
    {
        // Arrange
        var days = new List<WorkingDay> { CreateTestWorkingDay(DateTime.Today) };

        // Act
        var action = () => _strategy.Schedule(days, new List<TaskItem>());

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage("Must provide tasks to schedule");
    }

    [Fact]
    public void Schedule_WithSingleTaskAndAvailableSlot_SchedulesSuccessfully()
    {
        // Arrange
        var today = DateTime.Today;
        var workingDay = CreateTestWorkingDay(today);
        var task = CreateTestTask("Task1");

        // Act
        var result = _strategy.Schedule(new[] { workingDay }, new[] { task });

        // Assert
        result.ScheduledTasks.Should().HaveCount(1);
        result.UnscheduledTasks.Should().BeEmpty();
        result.ScheduledTasks[0].Name.Should().Be("Task1");
    }

    [Fact]
    public void Schedule_WithMultipleTasksAndLimitedTime_SchedulesHighestPriorityFirst()
    {
        // Arrange
        var today = DateTime.Today;
        var workingDay = CreateTestWorkingDay(today);
        var highPriorityTask = CreateTestTask("High Priority");
        var lowPriorityTask = CreateTestTask("Low Priority", PriorityLevel.Medium);

        // Act
        var result = _strategy.Schedule(
            new[] { workingDay },
            new[] { lowPriorityTask, highPriorityTask }
        );

        // Assert
        result.ScheduledTasks.Should().HaveCount(2);
        result.ScheduledTasks[0].Name.Should().Be("High Priority");
        result.ScheduledTasks[1].Name.Should().Be("Low Priority");
    }

    [Fact]
    public void Schedule_WithTaskAfterDueDate_MovesToUnscheduled()
    {
        // Arrange
        var futureDate = DateTime.Today.AddDays(5);
        var workingDay = CreateTestWorkingDay(futureDate);
        var task = CreateTestTask("Past Due", PriorityLevel.High, DateTime.Today.AddDays(-1));

        // Act
        var result = _strategy.Schedule(new[] { workingDay }, new[] { task });

        // Assert
        result.ScheduledTasks.Should().BeEmpty();
        result.UnscheduledTasks.Should().HaveCount(1);
        result.UnscheduledTasks[0].Name.Should().Be("Past Due");
    }

    [Fact]
    public void Schedule_WithExactlyFittingTimeSlot_UsesPerfectFit()
    {
        // Arrange
        var today = DateTime.Today;
        var workingDay = CreateTestWorkingDay(today);
        var task = CreateTestTask(
            "Perfect Fit",
            PriorityLevel.High,
            today.AddDays(1),
            TimeSpan.FromHours(3)
        );
        var testEvent = CreateTestEvent(new TimeOnly(13, 0), new TimeOnly(14, 0));
        workingDay.AddEvent(testEvent); //should schedule it at the end of the day instead of the beginning

        // Act
        var result = _strategy.Schedule(new[] { workingDay }, new[] { task });
        var scheduledTask = result.ScheduledTasks[0];

        // Assert
        result.ScheduledTasks.Should().HaveCount(1);
        scheduledTask.TimeSlot.Duration.Should().Be(TimeSpan.FromHours(3));
        scheduledTask
            .TimeSlot.Should()
            .Be(TimeSlot.Create(new TimeOnly(14, 0), new TimeOnly(17, 0)));
    }

    [Fact]
    public void Schedule_WithMultipleDays_SchedulesOnEarliestPossibleDay()
    {
        // Arrange
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var workingDays = new[] { CreateTestWorkingDay(tomorrow), CreateTestWorkingDay(today) };
        var task = CreateTestTask("Early Task");

        // Act
        var result = _strategy.Schedule(workingDays, new[] { task });
        var scheduledTask = result.ScheduledTasks[0];

        // Assert
        result.ScheduledTasks.Should().HaveCount(1);
        scheduledTask.TimeSlot.Start.Should().Be(new TimeOnly(9, 0)); // Default start time
        DateOnly
            .FromDateTime(today)
            .Should()
            .Be(workingDays.First(d => d.CalendarItems.Contains(scheduledTask)).DayDate);
    }

    private static WorkingDay CreateTestWorkingDay(DateTime date)
    {
        var timeSlot = TimeSlot.Create(new TimeOnly(9, 0), new TimeOnly(17, 0));
        return WorkingDay.Create(DateOnly.FromDateTime(date), timeSlot);
    }

    private TaskItem CreateTestTask(
        string name,
        PriorityLevel priorityLevel = PriorityLevel.High,
        DateTime? dueDate = null,
        TimeSpan? duration = null
    )
    {
        return new TaskItem(
            name,
            dueDate ?? DateTime.Today.AddDays(7),
            priorityLevel,
            _mockScoringStrategy,
            duration ?? TimeSpan.FromHours(2)
        );
    }

    private static Event CreateTestEvent(TimeOnly start, TimeOnly end)
    {
        var timeslot = TimeSlot.Create(start, end);
        return Event.Create(timeslot, new RecurrencePattern());
    }
}
