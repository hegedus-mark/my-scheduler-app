namespace Scheduler.Core.Models;

public class UserConfig
{
    public bool LongestJobFirst { get; set; }

    public UserConfig(bool longestJobFirst)
    {
        LongestJobFirst = longestJobFirst;
    }
}