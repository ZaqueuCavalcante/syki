namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

public class CreateWebhookSubscriptionService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<CreateWebhookSubscriptionOut, SykiError>> Create(Guid institutionId, CreateWebhookSubscriptionIn data)
    {
        if (data.Events?.Count == 0) return new InvalidWebhookEventsList();

        if (data.AuthenticationType == WebhookAuthenticationType.ApiKey && data.ApiKey.IsEmpty())
            return new InvalidWebhookAuthentication();

        var webhook = new WebhookSubscription(institutionId, data.Name, data.Url, data.Events, data.ApiKey);

        await ctx.SaveChangesAsync(webhook);

        return webhook.ToOut();
    }
}
