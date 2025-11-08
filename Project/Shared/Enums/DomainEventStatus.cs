namespace Exato.Shared.Enums;

/// <summary>
/// Status de um Evento de Domínio
/// </summary>
public enum DomainEventStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Processando")]
    Processing,

    [Description("Sucesso")]
    Success,

    [Description("Erro")]
    Error,
}
