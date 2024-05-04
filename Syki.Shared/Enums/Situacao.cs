using System.ComponentModel;

namespace Syki.Shared;

public enum Situacao
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
