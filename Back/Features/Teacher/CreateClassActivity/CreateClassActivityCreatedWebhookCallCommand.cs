using Syki.Back.Features.Academic.CallWebhooks;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

[CommandDescription("Criar chamada de webhook para o evento 'Atividade publicada'")]
public record CreateClassActivityCreatedWebhookCallCommand(Guid EventId, Guid WebhookId, Guid ClassActivityId) : ICommand;

public class CreateClassActivityCreatedWebhookCallCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateClassActivityCreatedWebhookCallCommand>
{
    public async Task Handle(Guid commandId, CreateClassActivityCreatedWebhookCallCommand command)
    {
        var activity = await ctx.ClassActivities
            .Where(x => x.Id == command.ClassActivityId)
            .Select(x => new { x.Id, x.Title, x.Type })
            .FirstAsync();

        var webhook = await ctx.Webhooks
            .Include(x => x.Authentication)
            .Where(x => x.Id == command.WebhookId)
            .Select(x => new { x.InstitutionId, x.Authentication.ApiKey })
            .FirstAsync();

        var payload = new ClassActivityCreatedWebhookPayload(activity.Id, activity.Title, activity.Type);
        var call = new WebhookCall(
            webhook.InstitutionId,
            command.WebhookId,
            payload,
            command.EventId,
            WebhookEventType.ClassActivityCreated
        );
        ctx.Add(call);
    }
}
