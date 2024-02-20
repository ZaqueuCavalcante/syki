using OtpNet;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;
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
        if (text.HasValue())
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }

        return "";
    }

    public static string ToSnakeCase(this string input)
    {
        if (input.IsEmpty()) { return ""; }

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

    public static bool IsValidEmail(this string email)
    {
        return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }

	private static JsonSerializerSettings _settings = new()
	{
		Converters = [new StringEnumConverter()],
	};
	public static string Serialize(this object obj)
	{
		return JsonConvert.SerializeObject(obj, _settings);
	}
}
