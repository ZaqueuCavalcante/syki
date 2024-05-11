using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Tests.Features.Academic.CreateNotification;

public class CreateUserNotificationUnitTests
{
    [Test]
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

    [Test]
    public void Should_create_notification_with_correct_user_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.UserId.Should().Be(userId);
    }

    [Test]
    public void Should_create_notification_with_correct_notification_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.NotificationId.Should().Be(notificationId);
    }

    [Test]
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
}
