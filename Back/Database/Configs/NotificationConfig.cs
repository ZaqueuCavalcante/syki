using Syki.Back.Domain;

namespace Syki.Back.Database;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> notification)
    {
        notification.ToTable("notifications");

        notification.HasKey(n => n.Id);
        notification.Property(n => n.Id).ValueGeneratedNever();

        notification.Ignore(n => n.Views);
    }
}
