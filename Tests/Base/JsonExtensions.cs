namespace Estud.Tests.Base;

public static class JsonExtensions
{
    public static async Task AssertBadRequest(this HttpResponseMessage httpResponse, EstudError estudError)
    {
        var error = await httpResponse.DeserializeTo<ErrorOut>();
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(estudError.Message);
        error.Code.Should().Be(estudError.Code);
    }

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, EstudError expected)
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

    public static void ShouldBeError<S>(this OneOf<S, EstudError> oneOf, EstudError expected)
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

    public static void ShouldBeError<S>(this OneOf<S, ErrorOut> oneOf, HttpStatusCode statusCode)
    {
        if (oneOf.IsSuccess)
        {
            throw new InvalidOperationException($"Expected HTTP status code '{statusCode}' but got success");
        }
        oneOf.IsError.Should().BeTrue();

        if (statusCode == HttpStatusCode.Unauthorized)
        {
            oneOf.Error.Code.Should().Be("401");
            oneOf.Error.Message.Should().Be("Unauthorized");
        }
        else if (statusCode == HttpStatusCode.Forbidden)
        {
            oneOf.Error.Code.Should().Be("403");
            oneOf.Error.Message.Should().Be("Forbidden");
        }
        else
        {
            oneOf.Error.Code.Should().Be(((int)statusCode).ToString());
        }
    }
}
