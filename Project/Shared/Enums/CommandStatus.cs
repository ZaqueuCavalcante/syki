namespace Exato.Shared.Enums;

/// <summary>
/// Status de um Comando
/// </summary>
public enum CommandStatus
{
    [Description("Pendente")]
    Pending,

    [Description("Aguardando")]
    Awaiting,

    [Description("Processando")]
    Processing,

    [Description("Sucesso")]
    Success,

    [Description("Erro")]
    Error,
}
