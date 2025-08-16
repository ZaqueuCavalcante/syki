namespace Syki.Back.Features.Academic.GetWebhookSubscription;

public class GetWebhookSubscriptionService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<GetWebhookSubscriptionOut, SykiError>> Get(Guid institutionId, Guid id)
    {
        var webhook = await ctx.Webhooks.AsNoTracking()
            .Include(w => w.Calls)
            .Include(w => w.Authentication)
            .Where(w => w.InstitutionId == institutionId && w.Id == id)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();

        if (webhook == null) return new WebhookNotFound();

        return webhook.ToGetWebhookSubscriptionOut();
    }
}
