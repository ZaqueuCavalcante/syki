namespace Syki.Tests.Base;

public static class JsonExtensions
{
    public static async Task AssertBadRequest(this HttpResponseMessage httpResponse, SykiError sykiError)
    {
        var error = await httpResponse.DeserializeTo<ErrorOut>();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(sykiError.Message);
        error.Code.Should().Be(sykiError.Code);
    }

    public static void ShouldBeError<T>(this OneOf<T, ErrorOut> current, SykiError expected)
    {
        current.GetError().Message.Should().Be(expected.Message);
        current.GetError().Code.Should().Be(expected.Code);
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
