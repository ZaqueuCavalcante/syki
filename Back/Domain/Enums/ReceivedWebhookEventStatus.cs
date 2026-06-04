namespace Syki.Back.Domain.Enums;

/// <summary>
/// Status de um evento de webhook recebido
/// </summary>
public enum ReceivedWebhookEventStatus
{
    [Description("Pendente")]
    Pending = 0,

    [Description("Processando")]
    Processing = 1,

    [Description("Processado")]
    Processed = 2,

    [Description("Erro")]
    Error = 3,

    [Description("Ignorado")]
    Ignored = 4,
}
