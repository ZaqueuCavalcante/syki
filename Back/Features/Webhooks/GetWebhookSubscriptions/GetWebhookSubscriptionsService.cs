namespace Estud.Back.Features.Webhooks.GetWebhookSubscriptions;

public class GetWebhookSubscriptionsService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetWebhookSubscriptionsOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var subscriptions = await ctx.WebhookSubscriptions.AsNoTracking()
            .Where(x => x.InstitutionId == institutionId)
            .OrderBy(x => x.Name)
            .ToListAsync();

        var items = subscriptions.ConvertAll(x => x.ToGetWebhookSubscriptionsItemOut());

        return new GetWebhookSubscriptionsOut { Total = items.Count, Items = items };
    }
}
