namespace Syki.Back.Features.Academic.GetWebhook;

public class GetWebhookService(SykiDbContext ctx) : IAcademicService
{
    public async Task<GetWebhookOut> Get(Guid institutionId, Guid id)
    {
        var webhook = await ctx.Webhooks.AsNoTracking()
            .Include(w => w.Calls)
            .Include(w => w.Authentication)
            .Where(w => w.InstitutionId == institutionId && w.Id == id)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefaultAsync();

        return webhook.ToGetWebhookOut();
    }
}
