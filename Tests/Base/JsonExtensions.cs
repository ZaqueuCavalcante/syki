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
        if (oneOf.IsSuccess)
        {
            throw new InvalidOperationException($"Expected error '{expected}' not found");
        }
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }

    public static void ShouldBeSuccess<S, E>(this OneOf<S, E> oneOf)
    {
        if (oneOf.IsError)
        {
            throw new InvalidOperationException($"'{oneOf.Error}'");
        }
        oneOf.IsSuccess.Should().BeTrue();
    }

    public static void ShouldBeError<S>(this OneOf<S, SykiError> oneOf, SykiError expected)
    {
        if (oneOf.IsSuccess)
        {
            throw new InvalidOperationException($"Expected error '{expected}' not found");
        }
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Should().BeOfType(expected.GetType());
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, ErrorOut expected)
    {
        if (oneOf.IsSuccess)
        {
            throw new InvalidOperationException($"Expected error '{expected}' not found");
        }
        oneOf.IsError.Should().BeTrue();
        oneOf.Error.Code.Should().Be(expected.Code);
        oneOf.Error.Message.Should().Be(expected.Message);
    }
}
