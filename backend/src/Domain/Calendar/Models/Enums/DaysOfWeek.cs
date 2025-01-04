namespace Domain.Calendar.Models.Enums;

[Flags]
public enum DaysOfWeek
{
    None = 0, // 0b0000000
    Monday = 1, // 0b0000001
    Tuesday = 2, // 0b0000010
    Wednesday = 4, // 0b0000100
    Thursday = 8, // 0b0001000
    Friday = 16, // 0b0010000
    Saturday = 32, // 0b0100000

    Sunday =
        64 // 0b1000000
    ,
}
