using BrazilHolidays.Net;
using System.Globalization;

namespace Estud.Back.Shared;

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

    public static string FormatBr(this DateOnly date)
    {
        return date.ToString("dd/MM/yyyy");
    }

    private static readonly DateOnly MinBirthdate = new(1900, 1, 1);

    public static bool IsValidBirthdate(this DateOnly date)
    {
        return date >= MinBirthdate && date <= DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
