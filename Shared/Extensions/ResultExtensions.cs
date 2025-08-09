namespace Syki.Shared;

public static class ResultExtensions
{
    extension<S, E>(OneOf<S, E> value)
    {
        public bool IsSuccess => value.IsT0;
        public S Success => value.IsSuccess
            ? value.AsT0
            : throw new InvalidOperationException($"{value.Error}");

        public bool IsError => value.IsT1;
        public E Error => value.IsError
            ? value.AsT1
            : throw new InvalidOperationException($"{value.Success}");
    }
}
