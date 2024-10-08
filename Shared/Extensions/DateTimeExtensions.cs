namespace Syki.Shared;

public static class DateTimeExtensions
{
    public static string Format(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyy HH:mm");
    }

    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }
}
