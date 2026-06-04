using Syki.Back.Database.Notifications;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureNotifications(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationDbConfig());
        modelBuilder.ApplyConfiguration(new UserNotificationDbConfig());
    }
}
