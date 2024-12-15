using Moq;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Services;
using Scheduler.Domain.Shared.Enums;
using Scheduler.Domain.Shared.Results;
using Scheduler.Domain.ValueObjects;

namespace Tests.Domain.Models;

public class ScheduleCalendarTests
{
    private readonly UserScheduleConfig _defaultConfig;
    private readonly Mock<ISchedulingStrategy> _mockSchedulingStrategy;
    private readonly DateOnly _today;

    public ScheduleCalendarTests()
    {
        _mockSchedulingStrategy = new Mock<ISchedulingStrategy>();
        _defaultConfig = UserScheduleConfig.CreateDefault();
        _today = DateOnly.FromDateTime(DateTime.Now);
    }

    [Fact]
    public void Create_WithValidConfig_ReturnsNewCalendar()
    {
        // Act
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);

        // Assert
        Assert.NotNull(calendar);
        Assert.NotEqual(Guid.Empty, calendar.Id);
    }

    [Fact]
    public void GetDaysInRange_ReturnsCorrectDays()
    {
        // Arrange
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);
        var startDate = _today;
        var endDate = startDate.AddDays(5);

        // Act
        var days = calendar.GetDaysInRage(startDate, endDate).ToList();

        // Assert
        Assert.Equal(6, days.Count); // Including both start and end dates
        Assert.All(days, day => Assert.True(day.DayDate >= startDate && day.DayDate <= endDate));
    }

    [Fact]
    public void GetWorkingDaysInRange_ExcludesNonWorkingDays()
    {
        // Arrange
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);
        var dateRange = DateRange.CreateFromDuration(_today, 0, 0, 7); // One week

        // Act
        var workingDays = calendar.GetWorkingDaysInRange(dateRange).ToList();

        // Assert
        Assert.All(workingDays, day => Assert.IsType<WorkingDay>(day));
        Assert.True(workingDays.Count <= 8);
    }

    [Fact]
    public void ScheduleTasks_WithValidTasks_CallsStrategy()
    {
        // Arrange
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);
        var tasks = CreateSampleTasks();
        var expectedResult = new SchedulingResult(new List<ScheduledTask>(), new List<TaskItem>());

        _mockSchedulingStrategy
            .Setup(s => s.Schedule(It.IsAny<IReadOnlyList<WorkingDay>>(), tasks, _defaultConfig))
            .Returns(expectedResult);

        // Act
        var result = calendar.ScheduleTasks(tasks);

        // Assert
        Assert.Same(expectedResult, result);
        _mockSchedulingStrategy.Verify(
            s => s.Schedule(It.IsAny<IReadOnlyList<WorkingDay>>(), tasks, _defaultConfig),
            Times.Once
        );
    }

    [Fact]
    public void ScheduleTasks_WithEmptyTaskList_ThrowsException()
    {
        // Arrange
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);
        var emptyTasks = new List<TaskItem>();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => calendar.ScheduleTasks(emptyTasks));
    }

    [Fact]
    public void ScheduleTasks_WithCustomWindow_UsesProvidedWindow()
    {
        // Arrange
        var calendar = ScheduleCalendar.Create(_defaultConfig, _mockSchedulingStrategy.Object);
        var tasks = CreateSampleTasks();
        var customWindow = DateRange.CreateFromDuration(_today, 0, 0, 3);

        // Act
        calendar.ScheduleTasks(tasks, customWindow);

        // Assert
        _mockSchedulingStrategy.Verify(
            s =>
                s.Schedule(
                    It.Is<IReadOnlyList<WorkingDay>>(days =>
                        days.All(d =>
                            d.DayDate >= customWindow.Start && d.DayDate <= customWindow.End
                        )
                    ),
                    tasks,
                    _defaultConfig
                ),
            Times.Once
        );
    }

    private IReadOnlyCollection<TaskItem> CreateSampleTasks()
    {
        var mockScoringStrategy = new Mock<IScoringStrategy>();
        return new List<TaskItem>
        {
            new(
                "Task 1",
                DateTime.Now.AddDays(2),
                PriorityLevel.High,
                mockScoringStrategy.Object,
                TimeSpan.FromHours(2)
            ),
            new(
                "Task 2",
                DateTime.Now.AddDays(3),
                PriorityLevel.Medium,
                mockScoringStrategy.Object,
                TimeSpan.FromHours(1)
            ),
        };
    }
}
