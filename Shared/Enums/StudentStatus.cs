using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Status do Aluno em um Curso
/// </summary>
public enum StudentStatus
{
    [Description("Matriculado")]
    Enrolled,

    [Description("Transferido")]
    Transferred,

    [Description("Trancado")]
    Deferred,

    [Description("Concluído")]
    Completed,
}
