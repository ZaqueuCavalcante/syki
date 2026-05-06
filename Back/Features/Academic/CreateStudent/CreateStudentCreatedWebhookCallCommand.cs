using System.Diagnostics;
using Syki.Back.Features.Academic.CallWebhooks;

namespace Syki.Back.Features.Academic.CreateStudent;

[CommandDescription("Criar chamada de webhook para o evento 'Aluno criado'")]
public record CreateStudentCreatedWebhookCallCommand(DomainEventId EventId, Guid WebhookId, Guid UserId) : ICommand;

public class CreateStudentCreatedWebhookCallCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateStudentCreatedWebhookCallCommand>
{
    public async Task Handle(CommandId commandId, CreateStudentCreatedWebhookCallCommand command)
    {
        var activityId = Activity.Current?.Id;

        var student = await ctx.Students.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.Id == command.UserId)
            .Select(x => new { x.Id, x.Name, x.User.Email, x.User.PhoneNumber, x.BirthDate })
            .FirstAsync();

        var webhook = await ctx.Webhooks.AsNoTracking()
            .Include(x => x.Authentication)
            .Where(x => x.Id == command.WebhookId)
            .Select(x => new { x.InstitutionId, x.Authentication.ApiKey })
            .FirstAsync();

        var payload = new StudentCreatedWebhookPayload(student.Id, student.Name, student.Email, student.PhoneNumber, student.BirthDate);
        var call = new WebhookCall(
            webhook.InstitutionId,
            command.WebhookId,
            payload,
            command.EventId,
            WebhookEventType.StudentCreated
        );
        ctx.Add(call);
    }
}
