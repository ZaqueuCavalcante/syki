using Syki.Back.Domain.Webhooks;

namespace Syki.Back.Database.Webhooks;

public class WebhookCallAttemptDbConfig : IEntityTypeConfiguration<WebhookCallAttempt>
{
    public void Configure(EntityTypeBuilder<WebhookCallAttempt> entity)
    {
        entity.ToTable("webhook_call_attempts", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
