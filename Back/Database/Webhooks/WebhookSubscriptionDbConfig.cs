using Newtonsoft.Json;
using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Database.Webhooks;

public class WebhookSubscriptionDbConfig : IEntityTypeConfiguration<WebhookSubscription>
{
    public void Configure(EntityTypeBuilder<WebhookSubscription> entity)
    {
        entity.ToTable("webhook_subscriptions", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Events)
            .HasConversion(
                v => v.Select(x => (int)x).ToList(),
                v => v.Select(x => (WebhookEventType)x).ToList()
            )
            .HasColumnType("integer[]")
            .IsRequired();

        entity.Property(e => e.CustomHeaders)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v) ?? new()
            )
            .HasColumnType("jsonb")
            .IsRequired();

        entity.HasMany(e => e.Calls)
            .WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(x => x.WebhookSubscriptionId);
    }
}
