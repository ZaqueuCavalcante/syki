namespace Syki.Back.Features.Academic.CallWebhooks;

public class WebhookCallAttemptConfig : IEntityTypeConfiguration<WebhookCallAttempt>
{
    public void Configure(EntityTypeBuilder<WebhookCallAttempt> webhookCallAttempt)
    {
        webhookCallAttempt.ToTable("webhook_call_attempts");

        webhookCallAttempt.HasKey(w => w.Id);
        webhookCallAttempt.Property(w => w.Id).ValueGeneratedNever();
    }
}
