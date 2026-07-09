namespace Estud.Back.Features.Webhooks.CreateWebhookSubscription;

public class CreateWebhookSubscriptionOut : IApiDto<CreateWebhookSubscriptionOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateWebhookSubscriptionOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = 1 }),
    ];
}
