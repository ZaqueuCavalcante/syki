namespace Syki.Back.Features.Academic.CallWebhooks;

public class WebhookCallConfig : IEntityTypeConfiguration<WebhookCall>
{
    public void Configure(EntityTypeBuilder<WebhookCall> webhookCall)
    {
        webhookCall.ToTable("webhook_calls");

        webhookCall.HasKey(w => w.Id);
        webhookCall.Property(w => w.Id).ValueGeneratedNever();

        webhookCall.HasMany(w => w.Attempts)
            .WithOne()
            .HasForeignKey(x => x.WebhookCallId);
    }
}
