namespace Exato.Shared.Features.Office.GetCommandBatchCommands;

public class GetCommandBatchCommandsIn
{
    public int Page { get; set; }
    public string? Type { get; set; }
    public CommandStatus? Status { get; set; }
}
