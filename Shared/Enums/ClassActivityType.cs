using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de Atividade
/// </summary>
public enum ClassActivityType
{
    [Description("Prova")]
    Exam,

    [Description("Prova")]
    Work,

    [Description("Apresentação")]
    Presentation,
}
