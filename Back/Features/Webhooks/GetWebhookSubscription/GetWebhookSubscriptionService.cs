namespace Syki.Back.Features.Webhooks.GetWebhookSubscription;

public class GetWebhookSubscriptionService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<GetWebhookSubscriptionOut, SykiError>> Get(int id)
    {
        var subscription = await ctx.WebhookSubscriptions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == id);
        if (subscription == null) return WebhookSubscriptionNotFound.I;

        return subscription.ToGetWebhookSubscriptionOut();
    }
}
