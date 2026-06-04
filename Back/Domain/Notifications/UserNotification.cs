namespace Syki.Back.Domain.Notifications;

public class UserNotification
{
    public int UserId { get; set; }
    public int NotificationId { get; set; }
    public DateTime? ViewedAt { get; set; }

    public Notification? Notification { get; set; }

    public UserNotification() {}

    public UserNotification(int userId, Notification notification)
    {
        UserId = userId;
        Notification = notification;
    }
}
