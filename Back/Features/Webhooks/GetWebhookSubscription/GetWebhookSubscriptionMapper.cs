using Syki.Back.Domain.Webhooks;

namespace Syki.Back.Features.Webhooks.GetWebhookSubscription;

public static class GetWebhookSubscriptionMapper
{
    extension(WebhookSubscription subscription)
    {
        public GetWebhookSubscriptionOut ToGetWebhookSubscriptionOut()
        {
            return new()
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Url = subscription.Url,
                IsActive = subscription.IsActive,
                Events = subscription.Events,
                CustomHeaders = subscription.CustomHeaders,
                CreatedAt = subscription.CreatedAt,
            };
        }
    }
}
