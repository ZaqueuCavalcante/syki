namespace Exato.Shared.Extensions;

public static class DateOnlyExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    public static DateOnly? ToDateOnly(this DateTime? dateTime)
    {
        if (dateTime == null) return null;

        return dateTime.Value.ToDateOnly();
    }
}
