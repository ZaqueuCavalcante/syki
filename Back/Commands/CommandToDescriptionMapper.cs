namespace Syki.Back.Commands;

public static class CommandToDescriptionMapper
{
    public static string ToCommandDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(ICommand).Assembly.GetType(value)!;
        var customAttributes = (CommandDescriptionAttribute[])type.GetCustomAttributes(typeof(CommandDescriptionAttribute), true);

        return customAttributes[0].Description;
    }
}
