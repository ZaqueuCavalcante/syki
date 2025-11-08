namespace Exato.Back.Commands;

public class CommandBatch
{
    public Guid Id { get; set; }
    public int OrganizationId { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Id do evento que gerou o lote.
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Id do comando que gerou o lote.
    /// </summary>
    public Guid? SourceCommandId { get; set; }

    /// <summary>
    /// Id do comando que será executado quando o lote for processado com sucesso.
    /// </summary>
    public Guid? NextCommandId { get; set; }

    /// <summary>
    /// Quantidade de comandos dentro do lote.
    /// </summary>
    public int Size { get; set; }

    private CommandBatch() { }

    public static CommandBatch New(
        int organizationId,
        CommandBatchType type,
        Guid? eventId = null,
        Guid? sourceCommandId = null
    ) {
        return new()
        {
            Type = type,
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.Now,
            OrganizationId = organizationId,
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
