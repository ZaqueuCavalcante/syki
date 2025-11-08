using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Exato.Shared.Extensions;

public static class ObjectExtensions
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        Converters = [new StringEnumConverter()],
    };

    public static string Serialize(this object obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }
}
