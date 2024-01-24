using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Syki.Back.Configs;

public static class JsonConfigs
{
	private static JsonSerializerSettings _settings = new()
	{
		Converters = new List<JsonConverter>() { new StringEnumConverter() },
	};

	public static string Serialize(this object obj)
	{
		return JsonConvert.SerializeObject(obj, _settings);
	}
}
