namespace Syki.Shared;

public class CreateWebhookSubscriptionOut
{
    public Guid Id { get; set; }

    public static implicit operator CreateWebhookSubscriptionOut(OneOf<CreateWebhookSubscriptionOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
