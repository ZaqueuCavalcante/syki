namespace Syki.Back.Features.Academic.CreateNotification;

public class UserNotification
{
    public Guid UserId { get; set; }
    public Guid NotificationId { get; set; }
    public Notification Notification { get; set; }
    public DateTime? ViewedAt { get; set; }

    public UserNotification(Guid userId, Guid notificationId)
    {
        UserId = userId;
        NotificationId = notificationId;
    }

    public UserNotificationOut ToOut()
    {
        return new UserNotificationOut
        {
            NotificationId = NotificationId,
            Title = Notification.Title,
            Description = Notification.Description,
            CreatedAt = Notification.CreatedAt,
            ViewedAt = ViewedAt,
        };
    }
}
