namespace Exato.Shared.Features.Office.GetCommandBatchCommands;

public class GetCommandBatchCommandsOut
{
    public int Total { get; set; }
    public List<string> Types { get; set; }
    public List<GetCommandBatchCommandsItemOut> Items { get; set; }
}

public class GetCommandBatchCommandsItemOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
