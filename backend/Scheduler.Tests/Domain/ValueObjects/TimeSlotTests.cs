using Scheduler.Domain.Shared;

namespace Tests.Domain.ValueObjects;

public class TimeSlotTests
{
    // Let's test the creation of TimeSlots first
    [Fact]
    public void Create_WithValidTimes_CreatesTimeSlot()
    {
        // Arrange
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(10, 0);

        // Act
        var timeSlot = TimeSlot.Create(start, end);

        // Assert
        Assert.Equal(start, timeSlot.Start);
        Assert.Equal(end, timeSlot.End);
        Assert.Equal(TimeSpan.FromHours(1), timeSlot.Duration);
    }

    [Fact]
    public void Create_WithEndBeforeStart_ThrowsArgumentException()
    {
        // Arrange
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(9, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => TimeSlot.Create(start, end));
        Assert.Contains("Start time must be before end time", exception.Message);
    }

    [Fact]
    public void Create_WithEqualStartAndEnd_ThrowsArgumentException()
    {
        // Arrange
        var time = new TimeOnly(9, 0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => TimeSlot.Create(time, time));
        Assert.Contains("Start time must be before end time", exception.Message);
    }

    [Fact]
    public void Create_WithLessThanMinimumDuration_ThrowsArgumentException()
    {
        // Arrange
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(9, 0, 30); // Only 30 seconds difference

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => TimeSlot.Create(start, end));
        Assert.Contains("Duration must be at least 1 minute", exception.Message);
    }

    // Now let's test the Contains relationship between TimeSlots
    [Theory]
    [InlineData(9, 0, 10, 0, 9, 15, 9, 45)] // Completely contained
    [InlineData(9, 0, 10, 0, 9, 0, 10, 0)] // Equal boundaries
    public void Contains_WhenOtherSlotFitsWithin_ReturnsTrue(
        int outerStartHour,
        int outerStartMinute,
        int outerEndHour,
        int outerEndMinute,
        int innerStartHour,
        int innerStartMinute,
        int innerEndHour,
        int innerEndMinute
    )
    {
        // Arrange
        var outerSlot = TimeSlot.Create(
            new TimeOnly(outerStartHour, outerStartMinute),
            new TimeOnly(outerEndHour, outerEndMinute)
        );

        var innerSlot = TimeSlot.Create(
            new TimeOnly(innerStartHour, innerStartMinute),
            new TimeOnly(innerEndHour, innerEndMinute)
        );

        // Act & Assert
        Assert.True(outerSlot.Contains(innerSlot));
    }

    [Theory]
    [InlineData(9, 0, 10, 0, 8, 0, 9, 30)] // Starts before
    [InlineData(9, 0, 10, 0, 9, 30, 10, 30)] // Ends after
    [InlineData(9, 0, 10, 0, 8, 0, 11, 0)] // Completely encompasses
    public void Contains_WhenOtherSlotDoesNotFit_ReturnsFalse(
        int outerStartHour,
        int outerStartMinute,
        int outerEndHour,
        int outerEndMinute,
        int innerStartHour,
        int innerStartMinute,
        int innerEndHour,
        int innerEndMinute
    )
    {
        // Arrange
        var outerSlot = TimeSlot.Create(
            new TimeOnly(outerStartHour, outerStartMinute),
            new TimeOnly(outerEndHour, outerEndMinute)
        );

        var innerSlot = TimeSlot.Create(
            new TimeOnly(innerStartHour, innerStartMinute),
            new TimeOnly(innerEndHour, innerEndMinute)
        );

        // Act & Assert
        Assert.False(outerSlot.Contains(innerSlot));
    }

    // Let's test the Overlaps relationship between TimeSlots
    [Theory]
    [InlineData(9, 0, 10, 0, 9, 30, 10, 30)] // Partial overlap from start
    [InlineData(9, 0, 10, 0, 8, 30, 9, 30)] // Partial overlap from end
    [InlineData(9, 0, 10, 0, 8, 0, 11, 0)] // Complete overlap (contains)
    [InlineData(9, 0, 10, 0, 9, 0, 10, 0)] // Exact same times
    public void Overlaps_WhenSlotsShareTime_ReturnsTrue(
        int firstStartHour,
        int firstStartMinute,
        int firstEndHour,
        int firstEndMinute,
        int secondStartHour,
        int secondStartMinute,
        int secondEndHour,
        int secondEndMinute
    )
    {
        // Arrange
        var firstSlot = TimeSlot.Create(
            new TimeOnly(firstStartHour, firstStartMinute),
            new TimeOnly(firstEndHour, firstEndMinute)
        );

        var secondSlot = TimeSlot.Create(
            new TimeOnly(secondStartHour, secondStartMinute),
            new TimeOnly(secondEndHour, secondEndMinute)
        );

        // Act & Assert
        Assert.True(firstSlot.Overlaps(secondSlot));
        // Overlap relationship should be symmetric
        Assert.True(secondSlot.Overlaps(firstSlot));
    }

    [Theory]
    [InlineData(9, 0, 10, 0, 10, 0, 11, 0)] // Adjacent but not overlapping
    [InlineData(9, 0, 10, 0, 11, 0, 12, 0)] // Clearly separate
    public void Overlaps_WhenSlotsDoNotShareTime_ReturnsFalse(
        int firstStartHour,
        int firstStartMinute,
        int firstEndHour,
        int firstEndMinute,
        int secondStartHour,
        int secondStartMinute,
        int secondEndHour,
        int secondEndMinute
    )
    {
        // Arrange
        var firstSlot = TimeSlot.Create(
            new TimeOnly(firstStartHour, firstStartMinute),
            new TimeOnly(firstEndHour, firstEndMinute)
        );

        var secondSlot = TimeSlot.Create(
            new TimeOnly(secondStartHour, secondStartMinute),
            new TimeOnly(secondEndHour, secondEndMinute)
        );

        // Act & Assert
        Assert.False(firstSlot.Overlaps(secondSlot));
        // Non-overlap relationship should be symmetric
        Assert.False(secondSlot.Overlaps(firstSlot));
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        // Arrange
        var timeSlot = TimeSlot.Create(new TimeOnly(9, 30), new TimeOnly(10, 45));

        // Act
        var result = timeSlot.ToString();

        // Assert
        Assert.Equal("09:30 - 10:45", result);
    }
}
