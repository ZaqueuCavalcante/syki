namespace Syki.Back.Commands;

public class ProcessedCommand
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
    public Guid? EventId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? OriginalId { get; set; }
    public Guid? BatchId { get; set; }
}
