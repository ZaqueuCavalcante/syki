namespace Syki.Shared;

public class CreateWebhookSubscriptionOut : IApiDto<CreateWebhookSubscriptionOut>
{
    public Guid Id { get; set; }

    public static IEnumerable<(string, CreateWebhookSubscriptionOut)> GetExamples() =>
    [
        ("Exemplo", new() { Id = Guid.CreateVersion7() }),
    ];

    public static implicit operator CreateWebhookSubscriptionOut(OneOf<CreateWebhookSubscriptionOut, ErrorOut> value)
    {
        return value.Success;
    }
}
