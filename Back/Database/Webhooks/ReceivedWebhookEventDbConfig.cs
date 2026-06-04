using Syki.Back.Domain.Webhooks;
using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database.Webhooks;

public class ReceivedWebhookEventDbConfig : IEntityTypeConfiguration<ReceivedWebhookEvent>
{
    public void Configure(EntityTypeBuilder<ReceivedWebhookEvent> entity)
    {
        entity.ToTable("received_webhook_events", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Command)
            .WithOne()
            .HasPrincipalKey<Command>(c => c.Id)
            .HasForeignKey<ReceivedWebhookEvent>(e => e.CommandId);

        entity.HasIndex(e => new { e.ExternalId, e.Source })
            .IsUnique();
    }
}
