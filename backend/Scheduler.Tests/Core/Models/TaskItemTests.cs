using Moq;
using Scheduler.Core.Enum;
using Scheduler.Core.Models;
using Scheduler.Core.Models.Scoring;

namespace Tests.Core.Models;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesTask()
    {
        // Arrange
        var setup = new TestSetup(42);
        var name = "Important Task";
        var dueDate = DateTime.Today.AddDays(1);
        var priority = PriorityLevel.High;
        var duration = TimeSpan.FromHours(2);

        // Act
        var task = setup.CreateTask(name, dueDate, priority, duration);

        // Assert
        Assert.Equal(name, task.Name);
        Assert.Equal(dueDate, task.DueDate);
        Assert.Equal(priority, task.PriorityLevel);
        Assert.Equal(duration, task.Duration);
        Assert.Equal(42, task.Score);
    }

    [Fact]
    public void Constructor_WithZeroDuration_ThrowsArgumentException()
    {
        // Arrange
        var setup = new TestSetup();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => setup.CreateTask(duration: TimeSpan.Zero)
        );
        Assert.Contains("Duration must be positive", exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-24)]
    public void Constructor_WithNegativeDuration_ThrowsArgumentException(int hours)
    {
        // Arrange
        var setup = new TestSetup();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => setup.CreateTask(duration: TimeSpan.FromHours(hours))
        );
        Assert.Contains("Duration must be positive", exception.Message);
    }

    [Fact]
    public void CompareTo_WithDifferentScores_SortsInDescendingOrder()
    {
        // Arrange
        var setup = new TestSetup();
        var highScoreTask = setup.CreateTask(score: 100);
        var lowScoreTask = setup.CreateTask(score: 50);

        // Act & Assert
        Assert.True(
            highScoreTask.CompareTo(lowScoreTask) < 0,
            "Higher score task should come before lower score task"
        );
    }

    [Fact]
    public void CompareTo_WithEqualScores_SortsByDueDate()
    {
        // Arrange
        var setup = new TestSetup();
        var earlierTask = setup.CreateTask(dueDate: DateTime.Today.AddDays(1), score: 100);
        var laterTask = setup.CreateTask(dueDate: DateTime.Today.AddDays(2), score: 100);

        // Act & Assert
        Assert.True(
            earlierTask.CompareTo(laterTask) < 0,
            "Task with earlier due date should come first when scores are equal"
        );
    }

    [Fact]
    public void CompareTo_WithEqualScoresAndDueDates_SortsByPriorityDescending()
    {
        // Arrange
        var setup = new TestSetup();
        var dueDate = DateTime.Today.AddDays(1);
        var highPriorityTask = setup.CreateTask(
            dueDate: dueDate,
            priority: PriorityLevel.High,
            score: 100
        );
        var lowPriorityTask = setup.CreateTask(
            dueDate: dueDate,
            priority: PriorityLevel.Low,
            score: 100
        );

        // Act & Assert
        Assert.True(
            highPriorityTask.CompareTo(lowPriorityTask) > 0,
            "Higher priority task should come first when scores and due dates are equal"
        );
    }

    [Fact]
    public void CompareTo_WithAllEqual_SortsByDurationAscending()
    {
        // Arrange
        var setup = new TestSetup();
        var dueDate = DateTime.Today.AddDays(1);
        var shortTask = setup.CreateTask(
            dueDate: dueDate,
            priority: PriorityLevel.High,
            duration: TimeSpan.FromHours(1),
            score: 100
        );
        var longTask = setup.CreateTask(
            dueDate: dueDate,
            priority: PriorityLevel.High,
            duration: TimeSpan.FromHours(2),
            score: 100
        );

        // Act & Assert
        Assert.True(
            shortTask.CompareTo(longTask) < 0,
            "Shorter task should come first when all other criteria are equal"
        );
    }

    [Fact]
    public void CompareTo_WithNull_ReturnsOne()
    {
        // Arrange
        var setup = new TestSetup();
        var task = setup.CreateTask();

        // Act & Assert
        Assert.Equal(1, task.CompareTo(null));
    }

    private class TestSetup
    {
        public TestSetup(int defaultScore = 0)
        {
            MockScoring = new Mock<IScoringStrategy>();
            MockScoring.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(defaultScore);
        }

        public Mock<IScoringStrategy> MockScoring { get; }

        public TaskItem CreateTask(
            string name = "Test Task",
            DateTime? dueDate = null,
            PriorityLevel priority = PriorityLevel.Medium,
            TimeSpan? duration = null,
            int? score = null
        )
        {
            // If a specific score is provided, update the mock
            if (score.HasValue)
                MockScoring.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(score.Value);

            return new TaskItem(
                name,
                dueDate ?? DateTime.Today.AddDays(1),
                priority,
                MockScoring.Object,
                duration ?? TimeSpan.FromHours(1)
            );
        }
    }
}
