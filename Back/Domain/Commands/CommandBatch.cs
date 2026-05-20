using Syki.Back.Domain.Enums;

namespace Syki.Back.Commands.Domain.Commands;

public class CommandBatch
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Id do comando que gerou o lote
    /// </summary>
    public int? SourceCommandId { get; set; }

    /// <summary>
    /// Id do comando que será executado quando o lote for processado com sucesso
    /// </summary>
    public int? NextCommandId { get; set; }

    public int Size { get; set; }

    private CommandBatch() { }

    public static CommandBatch New(
        int institutionId,
        CommandBatchType type,
        int? sourceCommandId = null)
    {
        return new()
        {
            Type = type,
            CreatedAt = DateTime.UtcNow,
            InstitutionId = institutionId,
            Status = CommandBatchStatus.Pending,
            SourceCommandId = sourceCommandId,
        };
    }

    public void ContinueWith(Command command)
    {
        NextCommandId = command.Id;
        command.SetAwaiting();
    }
}
