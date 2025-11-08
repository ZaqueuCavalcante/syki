using System.Text;
using System.Reflection;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.WebUtilities;

namespace Exato.Shared.Extensions;

public static partial class StringExtensions
{
    public static bool IsEmpty(this string? text)
    {
        return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }

    public static bool IsEmpty(this StringValues text)
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

    public static bool HasValue(this StringValues text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static string OnlyNumbers(this string? text)
    {
        if (text.HasValue())
        {
            return new string(text!.Where(char.IsDigit).ToArray());
        }

        return "";
    }

    public static string ToIntFormat(this int value)
    {
        return value.ToString("N0", CultureInfo.CreateSpecificCulture("pt-BR")).Replace(",", ".");
    }

    public static int? ToInt(this string? txt)
    {
        if (txt.IsEmpty() || txt.OnlyNumbers().IsEmpty()) return null;

        return int.Parse(txt.OnlyNumbers());
    }

    public static string ToDateString(this DateTime date)
    {
        if (date == DateTime.MinValue)
            return "-";

        return date.ToString("dd/MM/yyyy");
    }

    public static string ToDateString(this DateTime? date)
    {
        if (date == null) return "-";

        return date.Value.ToString("dd/MM/yyyy");
    }

    public static string ToTimeString(this DateTime date)
    {
        if (date == DateTime.MinValue)
            return "-";

        return date.ToString("HH:mm:ss");
    }

    public static string ToTimeString(this DateTime? date)
    {
        if (date == null) return "-";

        return date.Value.ToString("HH:mm:ss");
    }

    public static string ToDateTimeString(this DateTime date)
    {
        if (date == DateTime.MinValue)
            return "-";

        return date.ToString("dd/MM/yyyy HH:mm:ss");
    }

    public static string ToDateTimeString(this DateTime? date)
    {
        if (date == null) return "-";
        
        if (date == DateTime.MinValue)
            return "-";

        return date.Value.ToString("dd/MM/yyyy HH:mm:ss");
    }

    public static string ToSnakeCase(this string input)
    {
        if (input.IsEmpty()) { return ""; }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string ToMaskedToken(this string input)
    {
        if (input.IsEmpty()) { return ""; }

        return $"{input[..4]}*****{input[^6..]}";
    }

    public static string AddQueryString(this string path, object obj)
    {
        return QueryHelpers.AddQueryString(path, ConvertObjectToDictionary(obj));
    }

    private static Dictionary<string, string?> ConvertObjectToDictionary(object obj)
    {
        if (obj == null) return [];

        Dictionary<string, string?> dictionary = [];
        PropertyInfo[] properties = obj.GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            object propertyValue = property.GetValue(obj)!;

            if (propertyValue != null)
            {
                var valueAsString = propertyValue.ToString();

                if (property.PropertyType == typeof(string))
                    valueAsString = ((string)propertyValue).HasValue() ? (string)propertyValue : null;

                if (property.PropertyType == typeof(DateTime))
                    valueAsString = ((DateTime)propertyValue).ToString("yyyy-MM-ddTHH:mm:sszzz");

                if (property.PropertyType == typeof(DateOnly))
                    valueAsString = ((DateOnly)propertyValue).ToString("yyyy-MM-dd");

                dictionary.Add(propertyName, valueAsString);
            }
        }

        return dictionary;
    }

    public static string GetSqlSpanName(this string sql)
    {
        var comparer = StringComparison.InvariantCultureIgnoreCase;
        var insert = sql.Contains("INSERT", comparer);
        var update = sql.Contains("UPDATE", comparer);
        var delete = sql.Contains("DELETE", comparer);
        var select = sql.Contains("SELECT", comparer);

        var builder = new StringBuilder();

        if (insert) builder.Append("INSERT ");
        if (update) builder.Append("UPDATE ");
        if (delete) builder.Append("DELETE");

        if (!insert && !update && !delete && select) builder.Append("SELECT");

        return builder.ToString().Trim();
    }

    public static bool IsValidEmail(this string email)
    {
        if (email.IsEmpty()) return false;
        return IsValidEmailRegex().IsMatch(email);
    }

    [GeneratedRegex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex IsValidEmailRegex();
}
