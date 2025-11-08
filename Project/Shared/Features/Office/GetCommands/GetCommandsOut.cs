namespace Exato.Shared.Features.Office.GetCommands;

public class GetCommandsOut
{
    public int Total { get; set; }
    public List<string> Types { get; set; } = [];
    public List<GetCommandsItemOut> Items { get; set; } = [];
}

public class GetCommandsItemOut
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public CommandStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int Duration { get; set; }
}
