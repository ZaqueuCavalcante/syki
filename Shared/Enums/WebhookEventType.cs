using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Eventos que podem ser enviados via Webhook
/// </summary>
public enum WebhookEventType
{
    [Description("Aluno criado")]
    StudentCreated,

    [Description("Atividade publicada")]
    ClassActivityCreated,
}
