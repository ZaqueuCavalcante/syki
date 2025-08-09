using System.Collections.Concurrent;

namespace Syki.Back.Commands;

public static class CommandToDescriptionMapper
{
    private static readonly ConcurrentDictionary<string, CommandDescriptionAttribute> _attributes = new();

    public static string ToCommandDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        return _attributes.GetOrAdd(value, GetCommandDescriptionAttribute(value)).Description;
    }

    private static CommandDescriptionAttribute GetCommandDescriptionAttribute(this string value)
    {
        var type = typeof(ICommand).Assembly.GetType(value)!;
        var customAttributes = (CommandDescriptionAttribute[])type.GetCustomAttributes(typeof(CommandDescriptionAttribute), true);
        return customAttributes[0];
    }
}
