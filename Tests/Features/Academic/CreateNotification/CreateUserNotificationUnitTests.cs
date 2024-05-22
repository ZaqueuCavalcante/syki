using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Tests.Features.Academic.CreateNotification;

public class CreateUserNotificationUnitTests
{
    [Test]
    public void Should_create_notification_with_correct_data()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var notificationId = Guid.NewGuid();

        // Act
        var userNotification = new UserNotification(userId, notificationId);

        // Assert
        userNotification.UserId.Should().Be(userId);
        userNotification.NotificationId.Should().Be(notificationId);
        userNotification.ViewedAt.Should().BeNull();
    }

    [Test]
    public void Should_convert_user_notification_to_out()
    {
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";
        var target = UsersGroup.Students;
        var notification = new Notification(Guid.NewGuid(), title, description, target, true);

        var userId = Guid.NewGuid();
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
