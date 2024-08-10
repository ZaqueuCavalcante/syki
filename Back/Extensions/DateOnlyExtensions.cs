using BrazilHolidays.Net;

namespace Syki.Back.Extensions;

public static class DateOnlyExtensions
{
    public static bool IsHoliday(this DateOnly day)
    {
        return day.ToDateTime(TimeOnly.Parse("12:00")).IsHoliday();
    }
}
