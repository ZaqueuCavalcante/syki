namespace Syki.Back.Commands;

public class Command
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Duration { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessorId { get; set; }
    public string? Error { get; set; }

    /// <summary>
    /// Id do evento que gerou o comando
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Id do comando que gerou o comando
    /// Utilizado quando um comando gera outro em seu handler
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Id do comando com erro que gerou o comando atual
    /// Utilizado quando o comando original está com erro e é reprocessado
    /// O comando atual é uma cópia do original (imutabilidade)
    /// </summary>
    public Guid? OriginalId { get; set; }

    /// <summary>
    /// Id do lote que contém o comando
    /// </summary>
    public Guid? BatchId { get; set; }

    public DateTime? NotBefore { get; set; }

    public Command() { }

    public Command(
        Guid institutionId,
        object data,
        Guid? eventId = null,
        Guid? parentId = null,
        Guid? originalId = null,
        Guid? batchId = null,
        int? delaySeconds = null
    ) {
        Id = Guid.CreateVersion7();
        InstitutionId = institutionId;
        Type = data.GetType().ToString();
        Data = data.Serialize();
        CreatedAt = DateTime.UtcNow;
        EventId = eventId;
        ParentId = parentId;
        OriginalId = originalId;
        BatchId = batchId;
        NotBefore = delaySeconds != null ? DateTime.UtcNow.AddSeconds(delaySeconds.Value) : null;
    }

    public void SetAwaiting()
    {
        Status = CommandStatus.Awaiting;
    }

    public void Processed(double duration)
    {
        ProcessedAt = DateTime.UtcNow;
        Duration = Convert.ToInt32(duration);
        Status = Error.HasValue() ? CommandStatus.Error : CommandStatus.Success;
    }
}
