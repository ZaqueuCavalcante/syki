using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Database.Webhooks;

public class WebhookCallDbConfig : IEntityTypeConfiguration<WebhookCall>
{
    public void Configure(EntityTypeBuilder<WebhookCall> entity)
    {
        entity.ToTable("webhook_calls", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Attempts)
            .WithOne()
            .HasForeignKey(x => x.WebhookCallId);
    }
}
