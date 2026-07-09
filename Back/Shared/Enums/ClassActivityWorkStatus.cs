namespace Estud.Back.Shared;

/// <summary>
/// Status de uma entrega de atividade
/// </summary>
public enum ClassActivityWorkStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Entregue")]
    Delivered,

    [Description("Finalizado")]
    Finalized,
}
