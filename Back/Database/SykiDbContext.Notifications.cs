using Syki.Back.Domain.Notifications;
using Syki.Back.Database.Notifications;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }

    private static void ConfigureNotifications(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationDbConfig());
        modelBuilder.ApplyConfiguration(new UserNotificationDbConfig());
    }
}
