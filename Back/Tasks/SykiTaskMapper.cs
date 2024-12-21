namespace Syki.Back.Tasks;

public static class SykiTaskMapper
{
    public static string ToSykiTaskDescription(this string value)
    {
        if (value.IsEmpty()) return value;

        var type = typeof(ISykiTask).Assembly.GetType(value)!;
        var customAttributes = (SykiTaskDescriptionAttribute[])type.GetCustomAttributes(typeof(SykiTaskDescriptionAttribute), true);

        return customAttributes[0].Description;
    }
}
