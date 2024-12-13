using Moq;
using Scheduler.Core.Algo;
using Scheduler.Core.Enum;
using Scheduler.Core.Extensions;
using Scheduler.Core.Models;
using Scheduler.Core.Models.Scoring;
using WorkingDay = Scheduler.Core.Models.CalendarItems.WorkingDay;

namespace Tests.Core.Algo;

public class UserTaskSchedulerTests
{
    // Create a mock scoring strategy that we can reuse across tests
    private readonly Mock<IScoringStrategy> _mockScoringStrategy;
    private readonly UserTaskScheduler _scheduler;

    public UserTaskSchedulerTests()
    {
        _mockScoringStrategy = new Mock<IScoringStrategy>();

        _mockScoringStrategy.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(0);

        _scheduler = new UserTaskScheduler();
    }

    [Fact]
    public void ScheduleTasks_WithEqualScores_SchedulesByDueDate()
    {
        // Arrange
        var today = DateTime.Today;
        var days = new List<WorkingDay> { CreateTestDay(today) };

        var tasks = new List<TaskItem>
        {
            CreateTestTask(1, "Later Due Date", today.AddDays(3), TimeSpan.FromHours(1)),
            CreateTestTask(2, "Middle Due Date", today.AddDays(2), TimeSpan.FromHours(1)),
            CreateTestTask(3, "Earlier Due Date", today.AddDays(1), TimeSpan.FromHours(1)),
        };

        // Act
        var result = _scheduler.ScheduleTasks(days, tasks);

        // Assert
        Assert.Equal(3, result.ScheduledTasks.Count);
        Assert.Empty(result.UnscheduledTasks);

        // Verify tasks are scheduled in due date order
        Assert.Equal("Earlier Due Date", result.ScheduledTasks[0].Name);
        Assert.Equal("Middle Due Date", result.ScheduledTasks[1].Name);
        Assert.Equal("Later Due Date", result.ScheduledTasks[2].Name);
    }

    [Fact]
    public void ScheduleTasks_VerifyMockScoring_AllTasksHaveZeroScore()
    {
        // Arrange
        var today = DateTime.Today;
        var task = CreateTestTask(1, "Test Task", today.AddDays(1), TimeSpan.FromHours(1));

        // Assert
        Assert.Equal(0, task.Score);

        // Verify the mock was called
        _mockScoringStrategy.Verify(s => s.CalculateScore(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public void ScheduleTasks_WithSameDueDate_SchedulesByPriority()
    {
        // Arrange
        SetupPriorityBasedScoring();

        var today = DateTime.Today;
        var commonDueDate = today.AddDays(2); // Same due date for all tasks
        var days = new List<WorkingDay> { CreateTestDay(today) };

        // Create tasks with same due date but different priorities
        var tasks = new List<TaskItem>
        {
            CreateTestTask(
                1,
                "Low Priority Task",
                commonDueDate,
                TimeSpan.FromHours(1),
                PriorityLevel.Low
            ),
            CreateTestTask(
                2,
                "High Priority Task",
                commonDueDate,
                TimeSpan.FromHours(1),
                PriorityLevel.High
            ),
            CreateTestTask(3, "Medium Priority Task", commonDueDate, TimeSpan.FromHours(1)),
        };

        // Act
        var result = _scheduler.ScheduleTasks(days, tasks);

        // Assert
        Assert.Equal(3, result.ScheduledTasks.Count);
        Assert.Empty(result.UnscheduledTasks);

        // Verify tasks are scheduled in priority order
        Assert.Equal("High Priority Task", result.ScheduledTasks[0].Name);
        Assert.Equal("Medium Priority Task", result.ScheduledTasks[1].Name);
        Assert.Equal("Low Priority Task", result.ScheduledTasks[2].Name);

        // Verify time slot ordering
        Assert.True(
            result.ScheduledTasks[0].TimeSlot.Start < result.ScheduledTasks[1].TimeSlot.Start,
            "High priority task should be scheduled before medium priority task"
        );
        Assert.True(
            result.ScheduledTasks[1].TimeSlot.Start < result.ScheduledTasks[2].TimeSlot.Start,
            "Medium priority task should be scheduled before low priority task"
        );

        // Verify the mock was called for each task
        _mockScoringStrategy.Verify(
            s => s.CalculateScore(It.IsAny<TaskItem>()),
            Times.Exactly(tasks.Count),
            "Scoring strategy should be called once for each task"
        );
    }

    [Fact]
    public void ScheduleTasks_PriorityVsDueDate_VerifyScoreImpact()
    {
        // Arrange
        SetupPriorityBasedScoring();

        var today = DateTime.Today;
        var days = new List<WorkingDay> { CreateTestDay(today) };

        var tasks = new List<TaskItem>
        {
            // Earlier due date but lower priority
            CreateTestTask(
                1,
                "Earlier Due Low Priority",
                today.AddDays(1),
                TimeSpan.FromHours(1),
                PriorityLevel.Low
            ),
            // Later due date but higher priority
            CreateTestTask(
                2,
                "Later Due High Priority",
                today.AddDays(2),
                TimeSpan.FromHours(1),
                PriorityLevel.High
            ),
        };

        // Act
        var result = _scheduler.ScheduleTasks(days, tasks);

        // Assert
        Assert.Equal(2, result.ScheduledTasks.Count);
        Assert.Empty(result.UnscheduledTasks);

        // Verify the high priority task is scheduled first despite later due date
        Assert.Equal("Later Due High Priority", result.ScheduledTasks[0].OriginalTask.Name);
        Assert.Equal("Earlier Due Low Priority", result.ScheduledTasks[1].OriginalTask.Name);

        // Verify scheduling times
        Assert.True(
            result.ScheduledTasks[0].TimeSlot.Start < result.ScheduledTasks[1].TimeSlot.Start,
            "High priority task should be scheduled earlier in the day"
        );
    }

    private void SetupPriorityBasedScoring()
    {
        _mockScoringStrategy.Reset();

        // Configure scoring based on priority levels
        _mockScoringStrategy
            .Setup(s =>
                s.CalculateScore(It.Is<TaskItem>(t => t.PriorityLevel == PriorityLevel.High))
            )
            .Returns(100);

        _mockScoringStrategy
            .Setup(s =>
                s.CalculateScore(It.Is<TaskItem>(t => t.PriorityLevel == PriorityLevel.Medium))
            )
            .Returns(50);

        _mockScoringStrategy
            .Setup(s =>
                s.CalculateScore(It.Is<TaskItem>(t => t.PriorityLevel == PriorityLevel.Low))
            )
            .Returns(25);
    }

    private WorkingDay CreateTestDay(DateTime date, int startHour = 9, int endHour = 17)
    {
        var dateOnly = date.ToDateOnly();
        return WorkingDay.Create(
            dateOnly,
            TimeSlot.Create(new TimeOnly(startHour, 0), new TimeOnly(endHour, 0))
        );
    }

    private TaskItem CreateTestTask(
        int id,
        string name,
        DateTime dueDate,
        TimeSpan duration,
        PriorityLevel priority = PriorityLevel.Medium
    )
    {
        return new TaskItem(name, dueDate, priority, _mockScoringStrategy.Object, duration);
    }
}
