namespace Syki.Back.Features.Notifications.GetNotifications;

public class NotificationRow
{
    public int Id { get; set; }
    public short NotificationType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ViewedAt { get; set; }
    public string? Metadata { get; set; }
    public int TotalRows { get; set; }
}
