using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Status de um Comando
/// </summary>
public enum CommandStatus
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
