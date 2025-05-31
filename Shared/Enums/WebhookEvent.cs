using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Eventos que podem ser enviados via Webhook
/// </summary>
public enum WebhookEvent
{
    [Description("Aluno criado")]
    StudentCreated,

    [Description("Atividade publicada")]
    ClassActivityCreated,
}
