using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Notifications;

namespace Estud.Back.Database.Notifications;

public class UserNotificationDbConfig : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> entity)
    {
        entity.ToTable("user_notifications", DbSchemas.Estud);

        entity.HasKey(e => new { e.UserId, e.NotificationId });

        entity.HasOne<EstudUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne(e => e.Notification)
            .WithMany()
            .HasPrincipalKey(n => n.Id)
            .HasForeignKey(e => e.NotificationId);
    }
}
