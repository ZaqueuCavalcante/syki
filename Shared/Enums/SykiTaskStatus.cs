using System.ComponentModel;

namespace Syki.Shared;

public enum SykiTaskStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Processando")]
    Processing,

    [Description("Sucesso")]
    Success,

    [Description("Erro")]
    Error,
}
