using System.Text;
using Newtonsoft.Json;

namespace Syki.Back.Extensions;

public static class JsonExtensions
{
    public static StringContent ToStringContent(this object obj)
    {
        var serializedObject = JsonConvert.SerializeObject(obj);
        return new StringContent(serializedObject, Encoding.UTF8, "application/json");
    }

    public static async Task<T> DeserializeTo<T>(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseAsString)!;
    }
}