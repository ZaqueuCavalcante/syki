using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de autenticação para Webhooks
/// </summary>
public enum WebhookAuthenticationType
{
    [Description("ApiKey")]
    ApiKey,
}
