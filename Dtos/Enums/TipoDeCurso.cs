using System.ComponentModel;

namespace Syki.Dtos;

public enum TipoDeCurso
{
    [Description("Bacharelado")]
    Bacharelado,

    [Description("Licenciatura")]
    Licenciatura,

    [Description("Tecnólogo")]
    Tecnologo,

    [Description("Especialização")]
    Especializacao,

    [Description("Mestrado")]
    Mestrado,

    [Description("Doutorado")]
    Doutorado,

    [Description("Pós-Doutorado")]
    PosDoutorado,
}
