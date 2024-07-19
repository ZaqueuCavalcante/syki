using OneOf;
using System.Text;
using Newtonsoft.Json;

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

    public static void ShouldBeSuccess<S, E>(this OneOf<S, E> oneOf)
    {
        oneOf.IsT0.Should().BeTrue();
        oneOf.IsT1.Should().BeFalse();
    }

    public static void ShouldBeError<S>(this OneOf<S, SykiError> oneOf, SykiError error)
    {
        oneOf.IsT0.Should().BeFalse();
        oneOf.IsT1.Should().BeTrue();
        oneOf.AsT1.Should().BeOfType(error.GetType());
    }
}
