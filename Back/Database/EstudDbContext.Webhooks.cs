using Estud.Back.Domain.Webhooks;
using Estud.Back.Database.Webhooks;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<WebhookCall> WebhookCalls { get; set; }
    public DbSet<WebhookCallAttempt> WebhookCallAttempts { get; set; }

    public DbSet<WebhookSubscription> WebhookSubscriptions { get; set; }

    public DbSet<ReceivedWebhookEvent> ReceivedWebhookEvents { get; set; }

    private static void ConfigureWebhooks(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WebhookCallDbConfig());
        modelBuilder.ApplyConfiguration(new WebhookCallAttemptDbConfig());

        modelBuilder.ApplyConfiguration(new WebhookSubscriptionDbConfig());

        modelBuilder.ApplyConfiguration(new ReceivedWebhookEventDbConfig());
    }
}
