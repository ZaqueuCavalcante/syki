using System.ComponentModel;

namespace Syki.Shared;

public enum Situacao
{
    [Description("Cursando")]
    Cursando,

    [Description("Aprovado")]
    Aprovado,

    [Description("Reprovado")]
    Reprovado,
}
