namespace Syki.Back.Exceptions;

public static class Throw
{
    public const string DE000 = "O nome do aluno deve ter ao menos 3 caracteres.";
    public const string DE001 = "O nome do professor deve ter ao menos 3 caracteres.";
    public const string DE002 = "Curso não encontrado.";
    public const string DE003 = "Lista de disciplinas inválida.";
    public const string DE004 = "Disciplina não encontrada.";
    public const string DE005 = "Período não encontrado.";
    public const string DE006 = "Período inválido.";
    public const string DE007 = "Data de início de período inválida.";
    public const string DE008 = "Data de fim de período inválida.";
    public const string DE009 = "A data de início deve ser menor que a de fim de período.";
    public const string DE010 = "Campus não encontrado.";
    public const string DE011 = "Grade não encontrada.";
    public const string DE012 = "Oferta não encontrada.";
    public const string DE013 = "Role inválida.";
    public const string DE014 = "Instituição não encontrada.";
    public const string DE015 = "Senha fraca.";
    public const string DE016 = "Email inválido.";
    public const string DE017 = "Email já utilizado.";
    public const string DE018 = "Professor não encontrado.";
    public const string DE019 = "Usuário não encontrado.";
    public const string DE020 = "Reset token inválido.";
    public const string DE021 = "Horário inválido.";
    public const string DE022 = "Horários conflitantes.";
    public const string DE023 = "A data de início deve ser menor que a de fim de período de matrícula.";
    public const string DE024 = "Registro não encontrado.";
    public const string DE025 = "Token de registro inválido.";
    public const string DE026 = "Já existe um período com esse id.";
    public const string DE027 = "MFA token inválido.";
    public const string DE028 = "Turma não encontrada.";

    public static void Now(this string message)
    {
        throw new DomainException(message);
    }

    public static SwaggerExample<ErrorOut> ToSwaggerExampleErrorOut(this string message)
    {
        return SwaggerExample.Create(message, new ErrorOut { Message = message });
    }
}
