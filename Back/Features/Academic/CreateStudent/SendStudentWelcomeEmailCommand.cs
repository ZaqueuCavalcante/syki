namespace Syki.Back.Features.Academic.CreateStudent;

[CommandDescription("Enviar email de boas-vindas para o novo aluno")]
public record SendStudentWelcomeEmailCommand(Guid InstitutionId, Guid UserId) : ICommand;

public class SendStudentWelcomeEmailCommandHandler(SykiDbContext ctx) : ICommandHandler<SendStudentWelcomeEmailCommand>
{
    public async Task Handle(SendStudentWelcomeEmailCommand command)
    {
        var student = await ctx.Students
            .Where(x => x.Id == command.InstitutionId)
            .FirstOrDefaultAsync();
    }
}
