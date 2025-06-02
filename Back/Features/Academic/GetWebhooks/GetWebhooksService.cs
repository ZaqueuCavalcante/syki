namespace Syki.Back.Features.Academic.GetWebhooks;

public class GetWebhooksService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetWebhooksOut>> Get(Guid institutionId)
    {
        var webhooks = await ctx.Webhooks.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return webhooks.ConvertAll(c => c.ToGetWebhooksOut());
    }
}
