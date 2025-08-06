namespace Syki.Shared;

public static class ResultExtensions
{
    extension<S, E>(OneOf<S, E> value)
    {
        public bool IsSuccess => value.IsT0;
        public S Success => value.AsT0;

        public bool IsError => value.IsT1;
        public E Error => value.AsT1;
    }
}
