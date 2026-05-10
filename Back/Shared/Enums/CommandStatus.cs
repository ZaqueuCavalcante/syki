using System.ComponentModel;

namespace Syki.Back.Shared;

/// <summary>
/// Status de um Comando
/// </summary>
public enum CommandStatus
{
    [Description("Pendente")]
    Pending = 0,

    [Description("Aguardando")]
    Awaiting = 1,

    [Description("Processando")]
    Processing = 2,

    [Description("Sucesso")]
    Success = 3,

    [Description("Erro")]
    Error = 4,
}
