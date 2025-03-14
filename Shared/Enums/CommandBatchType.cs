using System.ComponentModel;

namespace Syki.Shared;

/// <summary>
/// Tipo de um Lote de comandos
/// </summary>
public enum CommandBatchType
{
    [Description("Enviar emails de nova atividade")]
    SendNewClassActivityEmailCommands,
}
