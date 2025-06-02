namespace Syki.Back.Features.Academic.GetWebhookCall;

public class GetWebhookCallService(SykiDbContext ctx) : IAcademicService
{
    public async Task<GetWebhookCallFullOut> Get(Guid institutionId, Guid id)
    {
        var webhookCall = await ctx.WebhookCalls.AsNoTracking()
            .Include(w => w.Attempts)
            .Where(w => w.InstitutionId == institutionId && w.Id == id)
            .FirstOrDefaultAsync();

        var webhookName = await ctx.Webhooks
            .Where(w => w.InstitutionId == institutionId && w.Id == webhookCall.WebhookId)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();

        return webhookCall.ToGetWebhookCallFullOut(webhookName);
    }
}
