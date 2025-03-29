using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de um Lote de comandos
/// </summary>
public enum CommandBatchType
{
    [Description("Envio de emails de nova atividade")]
    SendNewClassActivityEmailCommandsBatch,
}
