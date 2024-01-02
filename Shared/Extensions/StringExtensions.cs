using OtpNet;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syki.Shared;

public static class StringExtensions
{
    public static bool IsEmpty(this string? text)
    {
        return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }

    public static bool IsIn(this string? text, params string[] others)
    {
        if (text.IsEmpty())
            return true;

        foreach (var other in others)
        {
            if (other.Contains(text!, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    public static bool HasValue(this string? text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static string OnlyNumbers(this string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }

        return "";
    }

    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToMfaToken(this string key)
    {
        var totp = new Totp(Base32Encoding.ToBytes(key));
        return totp.ComputeTotp();
    }

    public static string Format(this decimal value)
    {
        return value.ToString("0.00", CultureInfo.InvariantCulture);
    }
}
