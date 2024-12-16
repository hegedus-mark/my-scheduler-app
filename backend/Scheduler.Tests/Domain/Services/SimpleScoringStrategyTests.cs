using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared.Enums;

namespace Tests.Domain.Services;

public class SimpleScoringStrategyTests
{
    [Fact]
    public void CalculateScore_DueDateComponent_ScoresHigherForUrgentTasks()
    {
        // Arrange
        var setup = new TestSetup();
        var duration = TimeSpan.FromHours(1); // Keep duration constant

        var urgentTask = setup.CreateTask(DateTime.Today.AddDays(1), duration);

        var laterTask = setup.CreateTask(DateTime.Today.AddDays(5), duration);

        // Act
        var urgentScore = setup.ScoringStrategy.CalculateScore(urgentTask);
        var laterScore = setup.ScoringStrategy.CalculateScore(laterTask);

        // Assert
        // An urgent task should score higher than a task due later
        Assert.True(
            urgentScore > laterScore,
            $"Urgent task score ({urgentScore}) should be higher than later task score ({laterScore})"
        );
    }

    [Theory]
    [InlineData(PriorityLevel.High, PriorityLevel.Medium)]
    [InlineData(PriorityLevel.Medium, PriorityLevel.Low)]
    [InlineData(PriorityLevel.High, PriorityLevel.Low)]
    public void CalculateScore_PriorityComponent_HigherPriorityScoresHigher(
        PriorityLevel higherPriority,
        PriorityLevel lowerPriority
    )
    {
        // Arrange
        var setup = new TestSetup();
        var dueDate = DateTime.Today.AddDays(7);
        var duration = TimeSpan.FromHours(1);

        var higherPriorityTask = setup.CreateTask(dueDate, duration, higherPriority);
        var lowerPriorityTask = setup.CreateTask(dueDate, duration, lowerPriority);

        // Act
        var higherPriorityScore = setup.ScoringStrategy.CalculateScore(higherPriorityTask);
        var lowerPriorityScore = setup.ScoringStrategy.CalculateScore(lowerPriorityTask);

        // Assert
        Assert.True(
            higherPriorityScore > lowerPriorityScore,
            $"Task with priority {higherPriority} ({higherPriorityScore}) "
                + $"should score higher than task with priority {lowerPriority} ({lowerPriorityScore})"
        );
    }

    [Fact]
    public void CalculateScore_CombinedFactors_ProducesExpectedResults()
    {
        // Arrange
        var setup = new TestSetup();

        // Create tasks with various combinations of factors
        var urgentHighPriorityLongTask = setup.CreateTask(
            DateTime.Today.AddDays(1),
            TimeSpan.FromHours(3),
            PriorityLevel.High
        );

        var laterLowPriorityShortTask = setup.CreateTask(
            DateTime.Today.AddDays(7),
            TimeSpan.FromHours(1),
            PriorityLevel.Low
        );

        // Act
        var urgentScore = setup.ScoringStrategy.CalculateScore(urgentHighPriorityLongTask);
        var laterScore = setup.ScoringStrategy.CalculateScore(laterLowPriorityShortTask);

        // Assert
        // The urgent, high-priority, long task should score significantly higher
        Assert.True(
            urgentScore > laterScore + 50,
            "Task with better characteristics in all aspects should score significantly higher"
        );
    }

    // We'll create a helper for common test scenarios
    private class TestSetup
    {
        public TestSetup()
        {
            ScoringStrategy = new SimpleScoringStrategy();
        }

        public SimpleScoringStrategy ScoringStrategy { get; }

        // Helper to create tasks with specific characteristics for testing
        public TaskItem CreateTask(
            DateTime dueDate,
            TimeSpan duration,
            PriorityLevel priority = PriorityLevel.Medium
        )
        {
            return new TaskItem("Test Task", dueDate, priority, ScoringStrategy, duration);
        }
    }
}
