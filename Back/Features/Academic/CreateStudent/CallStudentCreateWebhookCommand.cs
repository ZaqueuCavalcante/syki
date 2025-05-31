using Syki.Back.Features.Academic.CallWebhooks;

namespace Syki.Back.Features.Academic.CreateStudent;

[CommandDescription("Enviar email de boas-vindas para o novo aluno")]
public record CreateStudentCreatedWebhookCallCommand(Guid EventId, Guid WebhookId, Guid UserId) : ICommand;

public class CreateStudentCreatedWebhookCallCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateStudentCreatedWebhookCallCommand>
{
    public async Task Handle(Guid commandId, CreateStudentCreatedWebhookCallCommand command)
    {
        var student = await ctx.Students.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.Id == command.UserId)
            .Select(x => new { x.Id, x.Name, x.User.Email })
            .FirstAsync();

        var webhook = await ctx.Webhooks.AsNoTracking()
            .Include(x => x.Authentication)
            .Where(x => x.Id == command.WebhookId)
            .Select(x => new { x.InstitutionId, x.Url, x.Authentication.ApiKey })
            .FirstAsync();

        var payload = new StudentCreatedWebhookPayload(student.Id, student.Name, student.Email);
        var call = new WebhookCall(
            webhook.InstitutionId,
            command.WebhookId,
            webhook.Url,
            payload,
            command.EventId,
            WebhookEventType.StudentCreated
        );
        ctx.Add(call);
    }
}
