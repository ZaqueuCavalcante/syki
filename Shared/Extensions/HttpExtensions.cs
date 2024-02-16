using Newtonsoft.Json;

namespace Syki.Shared;

public static class HttpExtensions
{
    public static async Task<ErrorOut> ToError(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ErrorOut>(responseAsString)!;
    }
}
