using Syki.Back.CreateNotification;

namespace Syki.Back.Database;

public class UserNotificationConfig : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> userNotification)
    {
        userNotification.ToTable("users__notifications");

        userNotification.HasKey(un => new { un.UserId, un.NotificationId });

        userNotification.HasOne<SykiUser>()
            .WithMany()
            .HasForeignKey(un => un.UserId);

        userNotification.HasOne(x => x.Notification)
            .WithMany(x => x.Users)
            .HasForeignKey(un => un.NotificationId);
    }
}
