namespace Syki.Back.Features.Academic.CreateWebhookSubscription;

public class WebhookAuthenticationConfig : IEntityTypeConfiguration<WebhookAuthentication>
{
    public void Configure(EntityTypeBuilder<WebhookAuthentication> webhookAuthentication)
    {
        webhookAuthentication.ToTable("webhook_authentications");

        webhookAuthentication.HasKey(w => w.Id);
        webhookAuthentication.Property(w => w.Id).ValueGeneratedNever();
    }
}
