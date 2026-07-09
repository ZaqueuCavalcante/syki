namespace Estud.Back.Features.Webhooks.UpdateWebhookSubscription;

public class UpdateWebhookSubscriptionOut : IApiDto<UpdateWebhookSubscriptionOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, UpdateWebhookSubscriptionOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
