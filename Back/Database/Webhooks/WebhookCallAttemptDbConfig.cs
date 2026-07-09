using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Database.Webhooks;

public class WebhookCallAttemptDbConfig : IEntityTypeConfiguration<WebhookCallAttempt>
{
    public void Configure(EntityTypeBuilder<WebhookCallAttempt> entity)
    {
        entity.ToTable("webhook_call_attempts", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
