using Syki.Back.CreateNotification;

namespace Syki.Tests.Unit;

public class UserNotificationsUnitTests
{
    // [Test]
    public void Deve_criar_uma_user_notification_com_user_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.UserId.Should().Be(userId);
    }

    // [Test]
    public void Deve_criar_uma_user_notification_com_notification_id_correto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.NotificationId.Should().Be(notificationId);
    }

    // [Test]
    public void Deve_criar_uma_user_notification_com_viewed_at_nulo()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.ViewedAt.Should().BeNull();
    }

    // [Test]
    public void Deve_converter_a_user_notification_corretamente_pro_out()
    {
        var userId = Guid.NewGuid();

        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";
        var notification = new Notification(Guid.NewGuid(), title, description);

        var userNotification = new UserNotification(userId, notification.Id)
        {
            Notification = notification,
            ViewedAt = DateTime.Now.AddDays(1)
        };

        // Act
        var userNotificationOut = userNotification.ToOut();

        // Assert
        userNotificationOut.NotificationId.Should().Be(notification.Id);
        userNotificationOut.Title.Should().Be(notification.Title);
        userNotificationOut.Description.Should().Be(notification.Description);
        userNotificationOut.CreatedAt.Should().Be(notification.CreatedAt);
        userNotificationOut.ViewedAt.Should().Be(userNotification.ViewedAt);
    }
}
