using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Notifications;

namespace Syki.Back.Database.Notifications;

public class UserNotificationDbConfig : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> entity)
    {
        entity.ToTable("user_notifications", DbSchemas.Syki);

        entity.HasKey(e => new { e.UserId, e.NotificationId });

        entity.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne(e => e.Notification)
            .WithMany()
            .HasPrincipalKey(n => n.Id)
            .HasForeignKey(e => e.NotificationId);
    }
}
