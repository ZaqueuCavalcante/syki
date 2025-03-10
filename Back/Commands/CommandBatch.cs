namespace Syki.Back.Commands;

public class CommandBatch
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public CommandBatchStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? NextCommandId { get; set; }

    private CommandBatch() { }

    public static CommandBatch New()
    {
        return new CommandBatch
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Status = CommandBatchStatus.Processing
        };
    }

    public void ContinueWith(Command command)
    {
        NextCommandId = command.Id;
        command.SetAwaiting(Id);
    }
}
