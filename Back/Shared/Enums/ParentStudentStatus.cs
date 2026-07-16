namespace Estud.Back.Shared;

/// <summary>
/// Status do vínculo entre Responsável e Aluno
/// </summary>
public enum ParentStudentStatus
{
    [Description("Pendente")]
    Pending = 0,

    [Description("Ativo")]
    Active = 1,

    [Description("Revogado")]
    Revoked = 2,
}
