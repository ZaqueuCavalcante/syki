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

    [Description("Dispensado")]
    Dispensado,

    [Description("Reprovado")]
    Reprovado,
}
