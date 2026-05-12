namespace Syki.Back.Commands.Domain.Enums;

public enum CommandBatchStatus
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
