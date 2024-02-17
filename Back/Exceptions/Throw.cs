namespace Syki.Back.Exceptions;

public static class Throw
{
    // TODO: melhorar essas mensagens e criar especificas para cada erro
    public const string DE0000 = "Nome inválido.";
    public const string DE0001 = "Curso inválido.";
    public const string DE0002 = "Disciplina inválida.";
    public const string DE0003 = "Período inválido.";
    public const string DE0004 = "Data de início de período inválida.";
    public const string DE0005 = "Data de fim de período inválida.";
    public const string DE0006 = "A data de início deve ser menor que a de fim de período.";
    public const string DE0007 = "Campus inválido.";
    public const string DE0008 = "Grade inválida.";
    public const string DE0009 = "Oferta inválida.";
    public const string DE0010 = "Role inválida.";
    public const string DE0011 = "Faculdade inválida.";
    public const string DE0012 = "Senha fraca.";
    public const string DE0013 = "Email inválido.";
    public const string DE0014 = "Email duplicado.";
    public const string DE0015 = "Professor inválido.";
    public const string DE0016 = "Usuário não encontrado.";
    public const string DE0017 = "Reset token inválido.";
    public const string DE0018 = "Horário inválido.";

    public const string DE1100 = "Horários conflitantes.";
    public const string DE1101 = "A data de início deve ser menor que a de fim de período de matrícula.";
    public const string DE1102 = "Email já utilizado.";
    public const string DE1103 = "Demo não encontrada.";
    public const string DE1104 = "Demo token inválido.";
    public const string DE1105 = "Já existe um período com esse id.";

    public static void Now(this string message)
    {
        throw new DomainException(message);
    }
}
