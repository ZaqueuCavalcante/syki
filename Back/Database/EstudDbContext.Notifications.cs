using Estud.Back.Domain.Notifications;
using Estud.Back.Database.Notifications;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }

    private static void ConfigureNotifications(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationDbConfig());
        modelBuilder.ApplyConfiguration(new UserNotificationDbConfig());
    }
}
