namespace Exato.Shared.Features.Office.GetCommand;

public class GetCommandOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
    public string? Error { get; set; }

    public Guid? ParentId { get; set; }
    public Guid? OriginalId { get; set; }

    public List<GetCommandOut> Retries { get; set; } = [];
}
