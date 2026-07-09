using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Features.Webhooks.UpdateWebhookSubscription;

public class UpdateWebhookSubscriptionService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateWebhookSubscriptionIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidWebhookName.I);
            RuleFor(x => x.Name).MaximumLength(100).WithError(InvalidWebhookName.I);

            RuleFor(x => x.Url).NotEmpty().WithError(InvalidWebhookUrl.I);
            RuleFor(x => x.Url).Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithError(InvalidWebhookUrl.I);

            RuleFor(x => x.Events).Must(x => x != null && x.Count > 0).WithError(InvalidWebhookEvents.I);

            RuleFor(x => x.CustomHeaders).Must(WebhookCustomHeaders.IsValid).WithError(InvalidWebhookCustomHeaders.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateWebhookSubscriptionOut, EstudError>> Update(UpdateWebhookSubscriptionIn data)
    {
        if (V.Run(data, out var error)) return error;

        var subscription = await ctx.WebhookSubscriptions
            .FirstOrDefaultAsync(x => x.InstitutionId == ctx.RequestUser.InstitutionId && x.Id == data.Id);
        if (subscription == null) return WebhookSubscriptionNotFound.I;

        subscription.Update(data.Name, data.Url, data.Events, data.CustomHeaders, data.IsActive);
        await ctx.SaveChangesAsync();

        return new UpdateWebhookSubscriptionOut { Id = subscription.Id };
    }
}
