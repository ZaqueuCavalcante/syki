namespace Estud.Back.Domain.Enums;

/// <summary>
/// Eventos que podem ser enviados via Webhook
/// </summary>
public enum WebhookEventType
{
    [Description("Aluno criado")]
    StudentCreated = 0,

    [Description("Atividade publicada")]
    ClassActivityCreated = 1,
}
