using System.Net;
using System.Text;
using Syki.Shared;
using Newtonsoft.Json;
using FluentAssertions;

namespace Syki.Tests.Base;

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

    public static async Task AssertBadRequest(this HttpResponseMessage httpResponse, string message)
    {
        var error = await httpResponse.DeserializeTo<ErrorOut>();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(message);
    }
}
