using System.ComponentModel;

namespace Syki.Shared;

public enum WebhookCallStatus
{
    [Description("Sucesso")]
    Success = 0,

    [Description("Erro")]
    Error = 1,
}
