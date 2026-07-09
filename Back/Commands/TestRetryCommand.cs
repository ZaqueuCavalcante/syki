namespace Estud.Back.Commands;

[CommandDescription("Comando de teste que falha nas primeiras N execuções e depois tem sucesso.")]
public record TestRetryCommand(int FailUntilAttempt) : ICommand;

public class TestRetryCommandHandler(EstudDbContext ctx) : ICommandHandler<TestRetryCommand>
{
    public async Task Handle(int commandId, TestRetryCommand command)
    {
        var currentCommand = await ctx.Commands.AsNoTracking().FirstAsync(x => x.Id == commandId);

        // RetryAttempt is 0-based, FailUntilAttempt is 1-based (attempt 1 = first execution)
        var attemptNumber = currentCommand.RetryAttempt + 1;

        if (attemptNumber < command.FailUntilAttempt)
        {
            throw new Exception($"TestRetryCommand failed on attempt {attemptNumber}. Success only on attempt {command.FailUntilAttempt}.");
        }
    }
}
