namespace Syki.Shared;

public class UserNotificationOut
{
    public Guid NotificationId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ViewedAt { get; set; }
}
