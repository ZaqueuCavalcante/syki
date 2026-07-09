namespace Estud.Back.Features.Webhooks.GetWebhookSubscription;

public class GetWebhookSubscriptionService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetWebhookSubscriptionOut, EstudError>> Get(int id)
    {
        var subscription = await ctx.WebhookSubscriptions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == id);
        if (subscription == null) return WebhookSubscriptionNotFound.I;

        return subscription.ToGetWebhookSubscriptionOut();
    }
}
