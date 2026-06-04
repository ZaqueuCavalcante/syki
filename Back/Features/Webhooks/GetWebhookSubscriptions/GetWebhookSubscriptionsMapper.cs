using Syki.Back.Domain.Webhooks;

namespace Syki.Back.Features.Webhooks.GetWebhookSubscriptions;

public static class GetWebhookSubscriptionsMapper
{
    extension(WebhookSubscription subscription)
    {
        public GetWebhookSubscriptionsItemOut ToGetWebhookSubscriptionsItemOut()
        {
            return new()
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Url = subscription.Url,
                IsActive = subscription.IsActive,
                Events = subscription.Events,
                CreatedAt = subscription.CreatedAt,
            };
        }
    }
}
