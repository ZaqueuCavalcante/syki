namespace Syki.Back.Extensions;

public static class ResultExtensions
{
    extension <S, E>(OneOf<S, E> value)
    {
        public void ThrowOnError()
        {
            if (value.IsError) throw new Exception((value.Error as SykiError).Message);
        }
    }
}
