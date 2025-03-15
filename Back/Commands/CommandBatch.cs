namespace Syki.Back.Commands;

public class CommandBatch
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public CommandBatchType Type { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? NextCommandId { get; set; }
    public int Size { get; set; }

    private CommandBatch() { }

    public static CommandBatch New(Guid institutionId, CommandBatchType type)
    {
        return new()
        {
            Type = type,
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            InstitutionId = institutionId,
            Status = CommandBatchStatus.Pending,
        };
    }

    public void ContinueWith(Command command)
    {
        NextCommandId = command.Id;
        command.SetAwaiting();
    }
}
