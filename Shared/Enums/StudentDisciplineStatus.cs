using System.ComponentModel;

namespace Syki.Shared;

public enum StudentDisciplineStatus
{
    [Description("Pendente")]
    Pendente,

    [Description("Matriculado")]
    Matriculado,

    [Description("Aprovado")]
    Aprovado,

    [Description("Reprovado")]
    Reprovado,
}
