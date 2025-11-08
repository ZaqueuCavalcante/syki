namespace Exato.Web.Extensions;

public static class StringExtensions
{
    public static bool StringIsEmpty(this string? text)
    {
        return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }

    public static bool StringHasValue(this string? text)
    {
        return !text.StringIsEmpty();
    }
}
