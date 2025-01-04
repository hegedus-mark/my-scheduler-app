namespace Application.Scheduling.CalendarIntegration.DTOs;

public class GetAvailableSlotsRequest
{
    public DateTime WindowStart { get; init; }
    public DateTime WindowEnd { get; init; }
}
