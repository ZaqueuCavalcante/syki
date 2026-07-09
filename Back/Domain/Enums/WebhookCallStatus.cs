namespace Estud.Back.Domain.Enums;

public enum WebhookCallStatus
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
