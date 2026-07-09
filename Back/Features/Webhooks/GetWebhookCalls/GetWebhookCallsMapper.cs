using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Features.Webhooks.GetWebhookCalls;

public static class GetWebhookCallsMapper
{
    extension(WebhookCall call)
    {
        public GetWebhookCallsItemOut ToGetWebhookCallsItemOut()
        {
            return new()
            {
                Id = call.Id,
                EventType = call.EventType,
                Status = call.Status,
                AttemptsCount = call.AttemptsCount,
                CreatedAt = call.CreatedAt,
            };
        }
    }
}
