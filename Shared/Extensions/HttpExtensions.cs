using Newtonsoft.Json;

namespace Syki.Shared;

public static class HttpExtensions
{
    public static async Task<ErrorOut> ToError(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ErrorOut>(responseAsString)!;
    }

    public static async Task<T> DeserializeTo<T>(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseAsString)!;
    }

    public static async Task<OneOf<T, ErrorOut>> Resolve<T>(this HttpResponseMessage httpResponse)
    {
        if (httpResponse.IsSuccessStatusCode)
            return await httpResponse.DeserializeTo<T>();

        return await httpResponse.ToError();
    }
}
