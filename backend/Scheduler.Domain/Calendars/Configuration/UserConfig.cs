namespace Scheduler.Domain.Calendars.Configuration;

public class UserConfig
{
    public UserConfig(bool longestJobFirst)
    {
        LongestJobFirst = longestJobFirst;
    }

    public bool LongestJobFirst { get; set; }
}
