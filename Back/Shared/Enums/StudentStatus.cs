namespace Estud.Back.Shared;

/// <summary>
/// Status do Aluno em um Curso
/// </summary>
public enum StudentStatus
{
    [Description("Matriculado")]
    Enrolled = 0,

    [Description("Transferido")]
    Transferred = 1,

    [Description("Trancado")]
    Deferred = 2,

    [Description("Concluído")]
    Completed = 3,
}
