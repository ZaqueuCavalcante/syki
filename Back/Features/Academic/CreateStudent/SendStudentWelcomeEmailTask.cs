namespace Syki.Back.Features.Academic.CreateStudent;

[SykiTaskDescription("Enviar email de boas-vindas para o novo aluno")]
public record SendStudentWelcomeEmailTask(Guid UserId, Guid InstitutionId) : ISykiTask;

public class SendStudentWelcomeEmailTaskHandler(SykiDbContext ctx) : ISykiTaskHandler<SendStudentWelcomeEmailTask>
{
    public async Task Handle(SendStudentWelcomeEmailTask task)
    {
        var student = await ctx.Students
            .Where(x => x.Id == task.InstitutionId)
            .FirstOrDefaultAsync();
    }
}
