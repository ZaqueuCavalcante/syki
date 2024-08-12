using BrazilHolidays.Net;
using System.Globalization;

namespace Syki.Shared;

public static class DateOnlyExtensions
{
    public static bool IsHoliday(this DateOnly day)
    {
        return day.ToDateTime(TimeOnly.Parse("12:00")).IsHoliday();
    }

    public static DateOnly ToDateOnly(this string date)
    {
        return DateOnly.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }
}
