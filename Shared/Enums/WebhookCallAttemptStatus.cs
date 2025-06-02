using System.ComponentModel;

namespace Syki.Shared;

public enum WebhookCallAttemptStatus
{
    [Description("Sucesso")]
    Success = 0,

    [Description("Erro")]
    Error = 1,
}
