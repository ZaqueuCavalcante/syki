namespace Syki.Back.Commands;

public class CommandBatch
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Id do evento que gerou o lote
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Id do comando que gerou o lote
    /// </summary>
    public CommandId? SourceCommandId { get; set; }

    /// <summary>
    /// Id do comando que será executado quando o lote for processado com sucesso
    /// </summary>
    public CommandId? NextCommandId { get; set; }

    public int Size { get; set; }

    private CommandBatch() { }

    public static CommandBatch New(
        Guid institutionId,
        CommandBatchType type,
        Guid? eventId = null,
        CommandId? sourceCommandId = null)
    {
        return new()
        {
            Type = type,
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            InstitutionId = institutionId,
            Status = CommandBatchStatus.Pending,
            EventId = eventId,
            SourceCommandId = sourceCommandId,
        };
    }

    public void ContinueWith(Command command)
    {
        NextCommandId = command.Id;
        command.SetAwaiting();
    }
}
