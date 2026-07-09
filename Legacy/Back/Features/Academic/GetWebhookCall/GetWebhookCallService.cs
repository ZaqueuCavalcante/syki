namespace Estud.Back.Features.Academic.GetWebhookCall;

public class GetWebhookCallService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetWebhookCallFullOut, EstudError>> Get(Guid institutionId, Guid id)
    {
        var webhookCall = await ctx.WebhookCalls.AsNoTracking()
            .Include(w => w.Attempts)
            .Where(w => w.InstitutionId == institutionId && w.Id == id)
            .FirstOrDefaultAsync();
        
        if (webhookCall is null) return new WebhookCallNotFound();

        var webhookName = await ctx.Webhooks
            .Where(w => w.InstitutionId == institutionId && w.Id == webhookCall.WebhookId)
            .Select(w => w.Name)
            .FirstOrDefaultAsync();

        return webhookCall.ToGetWebhookCallFullOut(webhookName);
    }
}
