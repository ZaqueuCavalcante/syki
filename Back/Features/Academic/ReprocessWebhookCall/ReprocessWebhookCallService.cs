using Syki.Back.Features.Academic.CallWebhooks;

namespace Syki.Back.Features.Academic.ReprocessWebhookCall;

public class ReprocessWebhookCallService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Reprocess(Guid institutionId, Guid id)
    {
        var call = await ctx.WebhookCalls.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (call is null) return new WebhookCallNotFound();

        ctx.AddCommand(institutionId, new CallWebhookCommand(call.Id));
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
