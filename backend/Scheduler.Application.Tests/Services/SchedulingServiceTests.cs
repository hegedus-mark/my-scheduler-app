using FluentAssertions;
using Moq;
using Scheduler.Application.Commands.ScheduleTasks;
using Scheduler.Application.Entities;
using Scheduler.Application.Interfaces.Infrastructure;
using Scheduler.Application.Interfaces.Mapping;
using Scheduler.Application.Services;
using Scheduler.Domain.Interfaces;
using Scheduler.Domain.Models;
using Scheduler.Domain.Models.Configuration;
using Scheduler.Domain.Services;
using Scheduler.Shared.Enums;
using Scheduler.Shared.ValueObjects;

namespace Scheduler.Application.Tests.Services;

public class SchedulingServiceTests
{
    private readonly Mock<IMapper<ICalendarDay, DayEntity>> _dayMapperMock;
    private readonly Mock<IScoringStrategy> _scoringStrategyMock;
    private readonly SchedulingService _sut;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserScheduleConfig _userScheduleConfig;

    public SchedulingServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _scoringStrategyMock = new Mock<IScoringStrategy>();
        _dayMapperMock = new Mock<IMapper<ICalendarDay, DayEntity>>();
        _userScheduleConfig = UserScheduleConfig.CreateDefault();

        _sut = new SchedulingService(
            _unitOfWorkMock.Object,
            _scoringStrategyMock.Object,
            _userScheduleConfig,
            _dayMapperMock.Object
        );
    }

    [Fact]
    public async Task ScheduleTasksAsync_WithNoTasks_ReturnsValidationError()
    {
        // Arrange
        var command = new ScheduleTasksCommand(new List<TaskToSchedule>());

        // Act
        var result = await _sut.ScheduleTasksAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Code.Should().Be("Validation");
    }

    [Fact]
    public async Task ScheduleTasksAsync_WithNullTasks_ReturnsValidationError()
    {
        // Arrange
        var command = new ScheduleTasksCommand(null);

        // Act
        var result = await _sut.ScheduleTasksAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Code.Should().Be("Validation");
    }

    [Fact]
    public async Task ScheduleTasksAsync_WithValidTasks_SchedulesSuccessfully()
    {
        // Arrange
        var tasks = new List<TaskToSchedule>
        {
            new("Task 1", DateTime.Now.AddDays(1), PriorityLevel.High, TimeSpan.FromHours(2)),
        };
        var command = new ScheduleTasksCommand(tasks);

        SetupMocksForSuccessfulScheduling();

        // Act
        var result = await _sut.ScheduleTasksAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        VerifyMocksForSuccessfulScheduling();
    }

    [Fact]
    public async Task ScheduleTasksAsync_CreatesNonWorkingDaysCorrectly()
    {
        // Arrange
        var sunday = GetNextSunday();
        var tasks = new List<TaskToSchedule>
        {
            new("Weekend Task", sunday, PriorityLevel.High, TimeSpan.FromHours(2)),
        };
        var command = new ScheduleTasksCommand(tasks);

        SetupMocksForWeekendScheduling(sunday);

        // Act
        var result = await _sut.ScheduleTasksAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        VerifyWeekendDayCreation();
    }

    private void SetupMocksForSuccessfulScheduling()
    {
        _unitOfWorkMock
            .Setup(u => u.CalendarDays.GetDaysInRangeAsync(It.IsAny<DateRange>()))
            .ReturnsAsync(new List<DayEntity>());

        _scoringStrategyMock.Setup(s => s.CalculateScore(It.IsAny<TaskItem>())).Returns(100);

        _dayMapperMock
            .Setup(m => m.ToDomain(It.IsAny<DayEntity>()))
            .Returns(
                (DayEntity e) =>
                    WorkingDay.Create(
                        DateOnly.FromDateTime(e.Date),
                        TimeSlot.Create(
                            TimeOnly.FromDateTime(e.WorkStartTime),
                            TimeOnly.FromDateTime(e.WorkEndTime)
                        )
                    )
            );
    }

    private void VerifyMocksForSuccessfulScheduling()
    {
        _unitOfWorkMock.Verify(
            u => u.CalendarDays.AddRangeAsync(It.IsAny<IEnumerable<DayEntity>>()),
            Times.Once
        );
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    private DateTime GetNextSunday()
    {
        var date = DateTime.Now;
        while (date.DayOfWeek != DayOfWeek.Sunday)
            date = date.AddDays(1);
        return date;
    }

    private void SetupMocksForWeekendScheduling(DateTime sunday)
    {
        _unitOfWorkMock
            .Setup(u => u.CalendarDays.GetDaysInRangeAsync(It.IsAny<DateRange>()))
            .ReturnsAsync(new List<DayEntity>());

        _dayMapperMock
            .Setup(m => m.ToEntity(It.IsAny<ICalendarDay>()))
            .Returns(
                (ICalendarDay day) =>
                    new DayEntity
                    {
                        Id = Guid.NewGuid(),
                        Date = sunday,
                        IsWorkingDay = false,
                    }
            );
    }

    private void VerifyWeekendDayCreation()
    {
        _dayMapperMock.Verify(
            m => m.ToEntity(It.Is<ICalendarDay>(d => !d.IsWorkingDay)),
            Times.AtLeastOnce
        );
    }
}
