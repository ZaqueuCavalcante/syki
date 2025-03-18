using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Status de uma Atividade
/// </summary>
public enum ClassActivityStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Publicada")]
    Published,

    [Description("Finalizada")]
    Finalized,
}
