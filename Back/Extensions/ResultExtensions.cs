namespace Syki.Back.Extensions;

public static class ResultExtensions
{
    public static void ThrowOnError<S, E>(this OneOf<S, E> value)
    {
        if (value.IsError()) throw new Exception((value.GetError() as SykiError).Message);
    }
}
