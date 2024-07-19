namespace Syki.Back.Exceptions;

public static class Throw
{
    public const string DE021 = "Horário inválido.";
    public const string DE022 = "Horários conflitantes.";

    public static void Now(this string message)
    {
        throw new DomainException(message);
    }
}
