namespace Syki.Back.Exceptions;

public static class Throw
{
    public const string DE019 = "Usuário não encontrado.";
    public const string DE021 = "Horário inválido.";
    public const string DE022 = "Horários conflitantes.";
    public const string DE023 = "A data de início deve ser menor que a de fim de período de matrícula.";
    public const string DE024 = "Token de registro inválido.";
    public const string DE025 = "Usuário já cadastrado.";
    public const string DE028 = "Turma não encontrada.";

    public static void Now(this string message)
    {
        throw new DomainException(message);
    }
}
