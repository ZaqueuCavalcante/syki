using System.ComponentModel;

namespace Syki.Dtos;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var attribute = value.GetType().GetField(value.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
        if (attribute is DescriptionAttribute[] source && source.Any())
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
}
