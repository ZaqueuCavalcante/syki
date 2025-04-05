using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Status de uma Aula
/// </summary>
public enum ClassLessonStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Concluída")]
    Finalized,
}
