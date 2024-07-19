namespace Syki.Back.Exceptions;

public static class Throw
{
    public const string DE021 = "Horário inválido.";
    public const string DE022 = "Horários conflitantes.";
    public const string DE023 = "A data de início deve ser menor que a de fim de período de matrícula.";

    public static void Now(this string message)
    {
        throw new DomainException(message);
    }
}
