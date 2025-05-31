using System.ComponentModel;

namespace Syki.Shared;

public enum WebhookCallStatus
{
    [Description("Sucesso")]
    Success = 1,

    [Description("Erro")]
    Error = 2,
}
