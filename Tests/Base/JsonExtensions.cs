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

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, SykiError expected)
    {
        oneOf.IsSuccess.Should().BeFalse();
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }

    public static void ShouldBeSuccess<S, E>(this OneOf<S, E> oneOf)
    {
        oneOf.IsSuccess.Should().BeTrue();
        oneOf.IsError.Should().BeFalse();
    }

    public static void ShouldBeError<S>(this OneOf<S, SykiError> oneOf, SykiError expected)
    {
        oneOf.IsSuccess.Should().BeFalse();
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Should().BeOfType(expected.GetType());
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, ErrorOut expected)
    {
        oneOf.IsSuccess.Should().BeFalse();
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }
}
