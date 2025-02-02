using Domain.Calendar.Models.CalendarItems;
using Infrastructure.Calendar.Entities;

namespace Infrastructure.Calendar.Mapping;

public static class CalendarItemMappingExtensions
{
    public static CalendarItemEntity ToEntity(this CalendarItem item)
    {
        throw new NotImplementedException();
    }

    public static CalendarItem ToDomain(this CalendarItemEntity entity)
    {
        throw new NotImplementedException();
    }
}
