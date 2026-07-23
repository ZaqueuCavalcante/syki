namespace Estud.Back.Domain.Enums;

/// <summary>
/// Status de um Evento de Domínio
/// </summary>
public enum DomainEventStatus
{
    [Description("Pendente")]
    Pending = 0,

    [Description("Processando")]
    Processing = 1,

    [Description("Sucesso")]
    Success = 2,

    [Description("Erro")]
    Error = 3,
}
