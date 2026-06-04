using Syki.Back.Domain.Webhooks;

namespace Syki.Back.Features.Webhooks.CreateWebhookSubscription;

public class CreateWebhookSubscriptionService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CreateWebhookSubscriptionIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidWebhookName.I);
            RuleFor(x => x.Name).MaximumLength(100).WithError(InvalidWebhookName.I);

            RuleFor(x => x.Url).NotEmpty().WithError(InvalidWebhookUrl.I);
            RuleFor(x => x.Url).Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithError(InvalidWebhookUrl.I);

            RuleFor(x => x.Events).Must(x => x != null && x.Count > 0).WithError(InvalidWebhookEvents.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateWebhookSubscriptionOut, SykiError>> Create(CreateWebhookSubscriptionIn data)
    {
        if (V.Run(data, out var error)) return error;

        var subscription = new WebhookSubscription(ctx.RequestUser.InstitutionId, data.Name, data.Url, data.Events);
        await ctx.SaveChangesAsync(subscription);

        return new CreateWebhookSubscriptionOut { Id = subscription.Id };
    }
}
