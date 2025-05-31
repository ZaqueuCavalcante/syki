using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de autenticação do Webhook
/// </summary>
public enum WebhookAuthenticationType
{
    [Description("ApiKey")]
    ApiKey,
}
