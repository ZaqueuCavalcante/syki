namespace Syki.Back.Extensions;

public static class ResultExtensions
{
    public static bool IsSuccess<S, E>(this OneOf<S, E> value) => value.IsT0;
    public static S GetSuccess<S, E>(this OneOf<S, E> value) => value.AsT0;

    public static bool IsError<S, E>(this OneOf<S, E> value) => value.IsT1;
    public static E GetError<S, E>(this OneOf<S, E> value) => value.AsT1;
}
