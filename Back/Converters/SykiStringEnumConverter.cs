using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syki.Back.Converters;

public sealed class SykiStringEnumConverter : JsonConverterFactory
{
    public SykiStringEnumConverter() { }

    public override bool CanConvert(Type typeToConvert)
    {
        var underlying = Nullable.GetUnderlyingType(typeToConvert);
        return underlying is not null && underlying.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var enumType = Nullable.GetUnderlyingType(typeToConvert)!;
        var generic = typeof(NullableEnumConverter<>).MakeGenericType(enumType);
        return (JsonConverter)Activator.CreateInstance(generic)!;
    }

    private sealed class NullableEnumConverter<TEnum> : JsonConverter<TEnum?> where TEnum : struct, Enum
    {
        public NullableEnumConverter() { }

        public override TEnum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.String)
            {
                var str = reader.GetString();
                if (str.IsEmpty()) return null;

                if (Enum.TryParse<TEnum>(str, ignoreCase: true, out var parsed) &&
                    Enum.IsDefined(parsed))
                    return parsed;

                if (int.TryParse(str, out var n) &&
                    Enum.IsDefined(typeof(TEnum), n))
                    return (TEnum)Enum.ToObject(typeof(TEnum), n);

                return null;
            }

            if (reader.TokenType == JsonTokenType.Number &&
                reader.TryGetInt32(out var value) &&
                Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), value);
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, TEnum? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value.ToString());
        }
    }
}

