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

    public static async Task AssertBadRequest(this HttpResponseMessage httpResponse, SykiError sykiError)
    {
        var error = await httpResponse.DeserializeTo<ErrorOut>();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(sykiError.Message);
        error.Code.Should().Be(sykiError.Code);
    }

    public static void ShouldBeSuccess<S, E>(this OneOf<S, E> oneOf)
    {
        oneOf.IsSuccess().Should().BeTrue();
        oneOf.IsError().Should().BeFalse();
    }

    public static void ShouldBeError<S>(this OneOf<S, SykiError> oneOf, SykiError error)
    {
        oneOf.IsSuccess().Should().BeFalse();
        oneOf.IsError().Should().BeTrue();
        oneOf.GetError().Should().BeOfType(error.GetType());
    }
}
