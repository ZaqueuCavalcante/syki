using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Tests.Features.Academic.CreateNotification;

public class CreateNotificationUnitTests
{
    [Test]
    public void Should_create_notification_with_correct_data()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";
        var target = UsersGroup.Students;
        var timeless = true;

        // Act
        var notification = new Notification(institutionId, title, description, target, timeless);

        // Assert
        notification.Id.Should().NotBeEmpty();
        notification.InstitutionId.Should().Be(institutionId);
        notification.Title.Should().Be(title);
        notification.Description.Should().Be(description);
        notification.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Should_convert_notification_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";
        var target = UsersGroup.Students;
        var timeless = true;

        var notification = new Notification(institutionId, title, description, target, timeless)
        {
            Views = "2/15"
        };

        // Act
        var notificationOut = notification.ToOut();

        // Assert
        notificationOut.Id.Should().Be(notification.Id);
        notificationOut.Title.Should().Be(notification.Title);
        notificationOut.Description.Should().Be(notification.Description);
        notificationOut.Timeless.Should().Be(notification.Timeless);
        notificationOut.Target.Should().Be(notification.Target);
        notificationOut.CreatedAt.Should().Be(notification.CreatedAt);
        notificationOut.Views.Should().Be(notification.Views);
    }
}
