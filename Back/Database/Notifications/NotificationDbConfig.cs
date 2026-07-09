using Estud.Back.Domain.Notifications;

namespace Estud.Back.Database.Notifications;

public class NotificationDbConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> entity)
    {
        entity.ToTable("notifications", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
