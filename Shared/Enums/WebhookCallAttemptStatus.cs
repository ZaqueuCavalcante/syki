using System.ComponentModel;

namespace Syki.Shared;

public enum WebhookCallAttemptStatus
{
    [Description("Sucesso")]
    Success = 1,

    [Description("Erro")]
    Error = 2,
}
