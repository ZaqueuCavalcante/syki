using OtpNet;
using QRCoder;
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

    public static string GenerateTOTP(this string key)
    {
        var totp = new Totp(Base32Encoding.ToBytes(key));
        return totp.ComputeTotp();
    }

    public static string ToBase64(this string value)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    public static string Format(this decimal value)
    {
        return value.ToString("0.00", CultureInfo.InvariantCulture);
    }

    public static string Format(this int value)
    {
        return value.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
    }

    public static bool IsValidEmail(this string email)
    {
        if (email.IsEmpty()) return false;
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

    public static Byte[] GenerateQrCodeBytes(this string key, string email)
    {
        const string provider = "Syki";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(
            $"otpauth://totp/{provider}:{email}?secret={key}&issuer={provider}",
            QRCodeGenerator.ECCLevel.Q
        );
        
        var qrCode = new PngByteQRCode(qrCodeData);

        return qrCode.GetGraphic(20);
    }

    public static string MinutesToString(this int value)
    {
        var hours = value / 60;
        var minutes = value % 60;

        if (hours == 0 && minutes == 0) return "0";
        if (hours == 0) return $"{minutes}min";
        if (minutes == 0) return $"{hours}h";

        return $"{hours}h e {minutes}min";
    }

	public static string ToThousandSeparated(this int value)
	{
		return value.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR"));
	}

	public static string ToMinuteString(this DateTime date)
	{
		if (date == DateTime.MinValue)
			return "-";

		return date.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
	}

	public static string ToMinuteString(this DateTime? date)
	{
		if (date == null)
			return "-";

		return date.Value.ToMinuteString();
	}
}
