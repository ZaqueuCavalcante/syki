using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo do Curso
/// </summary>
public enum CourseType
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
