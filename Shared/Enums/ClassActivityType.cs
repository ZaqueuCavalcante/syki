using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de Atividade
/// </summary>
public enum ClassActivityType
{
    [Description("Prova")]
    Exam,

    [Description("Projeto")]
    Project,
    
    [Description("Trabalho")]
    Work,

    [Description("Apresentação")]
    Presentation,
}
