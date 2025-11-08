using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Exato.Front.Extensions;

public static class NavManagerExtensions
{
    extension(NavigationManager nav)
    {
        public void ApplyQueryToFilterState(object filterState)
        {
            var uri = nav.ToAbsoluteUri(nav.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);

            if (query.Count <= 0) return;

            foreach (var p in filterState.GetType().GetProperties().Where(p => p.CanWrite))
            {
                var key = p.Name;
                if (!query.TryGetValue(key, out var sv) || sv.Count == 0) continue;

                object? value = null;

                try
                {
                    value = GetPropValue(p.PropertyType, sv[0]!);
                }
                catch { }

                if (value is not null) p.SetValue(filterState, value);
            }
        }

        public void ClearQueryString()
        {
            var path = new Uri(nav.Uri).GetLeftPart(UriPartial.Path);
            nav.NavigateTo(path, replace: true);
        }

        public void UpdateQueryString(object filterState)
        {
            var uri = nav.ToAbsoluteUri(nav.Uri);

            var query = QueryHelpers.ParseQuery(uri.Query)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString(), StringComparer.OrdinalIgnoreCase);

            var filters = filterState.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var filter in filters)
            {
                if (!IsPrimitiveType(filter.PropertyType))
                    continue;

                var key = filter.Name;
                var val = filter.GetValue(filterState);

                if (val == null || val.ToString().IsEmpty())
                {
                    // Remove all keys with same name (ignoring case)
                    var keyToRemove = query.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
                    if (keyToRemove != null)
                        query.Remove(keyToRemove);
                }
                else
                {
                    // Update or insert the value using exact casing from the property
                    var existingKey = query.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
                    if (existingKey != null && existingKey != key)
                    {
                        query.Remove(existingKey); // Remove old case-variant key
                    }

                    var stringValue = val is DateTime dateTime ? dateTime.ToString("yyyy-MM-dd") : val.ToString();
                    query[key] = stringValue ?? "";
                }
            }

            var baseUri = uri.GetLeftPart(UriPartial.Path);
            var newUri = QueryHelpers.AddQueryString(baseUri, query);

            nav.NavigateTo(newUri, forceLoad: false);
        }

    }

    private static bool IsPrimitiveType(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        return underlyingType == typeof(string) ||
            underlyingType == typeof(int) ||
            underlyingType == typeof(bool) ||
            underlyingType == typeof(decimal) ||
            underlyingType.IsEnum ||
            underlyingType == typeof(DateTime) ||
            underlyingType == typeof(TimeSpan);
    }

    private static object GetPropValue(Type type, string str)
    {
        if (type == typeof(string)) return str;

        if (type == typeof(Guid) || type == typeof(Guid?))
            return Guid.TryParse(str, out var g) ? g : null;

        if (type == typeof(DateTime) || type == typeof(DateTime?))
        {
            if (DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var isoDate))
                return isoDate;
            return DateTime.TryParse(str, out var dt) ? dt : null;
        }

        if (type == typeof(TimeSpan) || type == typeof(TimeSpan?))
        {
            var formats = new[] { "c", @"hh\:mm", @"hh\:mm\:ss" };
            if (TimeSpan.TryParseExact(str, formats, CultureInfo.InvariantCulture, out var ts))
                return ts;
            
            return TimeSpan.TryParse(str, CultureInfo.InvariantCulture, out ts) ? ts : null;
        }

        if (Nullable.GetUnderlyingType(type)?.IsEnum == true)
            return Enum.TryParse(Nullable.GetUnderlyingType(type)!, str, ignoreCase: true, out var en) ? en : null;

        if (type.IsEnum)
            return Enum.TryParse(type, str, true, out var en) ? en : null;

        return Convert.ChangeType(str, Nullable.GetUnderlyingType(type) ?? type);
    }
}
