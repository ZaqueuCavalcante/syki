namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

public class WebhookSubscriptionConfig : IEntityTypeConfiguration<WebhookSubscription>
{
    public void Configure(EntityTypeBuilder<WebhookSubscription> webhookSubscription)
    {
        webhookSubscription.ToTable("webhook_subscriptions");

        webhookSubscription.HasKey(w => w.Id);
        webhookSubscription.Property(w => w.Id).ValueGeneratedNever();

        webhookSubscription.HasOne(w => w.Authentication)
            .WithOne()
            .HasForeignKey<WebhookAuthentication>(w => w.WebhookId)
            .IsRequired(false);
    }
}
