namespace Syki.Back.Features.Webhooks.GetWebhookCalls;

public class GetWebhookCallsService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetWebhookCallsOut> Get(GetWebhookCallsIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var query = ctx.WebhookCalls.AsNoTracking().Where(x => x.InstitutionId == institutionId);

        var total = await query.CountAsync();

        var calls = await query
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((data.Page - 1) * data.PageSize)
            .Take(data.PageSize)
            .ToListAsync();

        var items = calls.ConvertAll(x => x.ToGetWebhookCallsItemOut());

        return new GetWebhookCallsOut
        {
            Total = total,
            Page = data.Page,
            PageSize = data.PageSize,
            Items = items,
        };
    }
}
