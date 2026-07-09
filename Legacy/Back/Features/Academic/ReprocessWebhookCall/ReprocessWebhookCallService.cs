using Estud.Back.Features.Academic.CallWebhooks;

namespace Estud.Back.Features.Academic.ReprocessWebhookCall;

public class ReprocessWebhookCallService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Reprocess(Guid institutionId, Guid id)
    {
        var call = await ctx.WebhookCalls.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (call is null) return new WebhookCallNotFound();

        ctx.AddCommand(institutionId, new CallWebhookCommand(call.Id));
        await ctx.SaveChangesAsync();

        return new EstudSuccess();
    }
}
