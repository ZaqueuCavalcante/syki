using Syki.Back.Domain.Notifications;

namespace Syki.Back.Database.Notifications;

public class NotificationDbConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> entity)
    {
        entity.ToTable("notifications", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
