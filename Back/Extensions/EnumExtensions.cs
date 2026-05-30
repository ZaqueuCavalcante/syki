namespace Syki.Back.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var attribute = value.GetType()
            .GetField(value.ToString())!
            .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);

        if (attribute is DescriptionAttribute[] source && source.Length != 0)
        {
            return source.First().Description;
        }

        return value.ToString();
    }

    public static bool IsIn(this Enum source, params Enum[] valuesToCheck)
    {
        if (valuesToCheck == null || valuesToCheck.Length == 0)
        {
            return false;
        }

        return valuesToCheck.Contains(source);
    }

    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static T ToEnum<T>(this short value)
    {
        return (T)Enum.Parse(typeof(T), value.ToString(), true);
    }

    public static T IntToEnum<T>(this int value)
    {
        return (T)Enum.Parse(typeof(T), value.ToString(), true);
    }

    public static bool IsValid(this Enum value)
    {
        return Enum.IsDefined(value.GetType(), value);
    }

    public static int ToInt<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return Convert.ToInt32(enumValue);
    }

    public static short ToShort<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        return (short) enumValue.ToInt();
    }

    public static bool Is(this DayOfWeek dayOfWeek, Day day)
    {
        return day.ToString() == dayOfWeek.ToString();
    }

    public static int DiffInMinutes(this Hour hourA, Hour hourB)
    {
        if (hourA > hourB)
        {
            (hourA, hourB) = (hourB, hourA);
        }

        var hourAHour = int.Parse(hourA.ToString().Substring(1, 2));
        var hourAMinute = int.Parse(hourA.ToString().Substring(4, 2));

        var hourBHour = int.Parse(hourB.ToString().Substring(1, 2));
        var hourBMinute = int.Parse(hourB.ToString().Substring(4, 2));

        var hours = hourBHour - hourAHour;
        var minutes = hourBMinute - hourAMinute;

        return hours * 60 + minutes;
    }
}
