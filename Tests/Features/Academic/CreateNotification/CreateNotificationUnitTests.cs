using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Tests.Features.Academic.CreateNotification;

public class CreateNotificationUnitTests
{
    [Test]
    public void Should_create_a_notification_with_not_empty_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        // Act
        var notification = new Notification(institutionId, title, description);

        // Assert
        notification.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_notification_with_correct_institution_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        // Act
        var notification = new Notification(institutionId, title, description);

        // Assert
        notification.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Should_create_a_notification_with_correct_title()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        // Act
        var notification = new Notification(institutionId, title, description);

        // Assert
        notification.Title.Should().Be(title);
    }

    [Test]
    public void Should_create_a_notification_with_correct_description()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        // Act
        var notification = new Notification(institutionId, title, description);

        // Assert
        notification.Description.Should().Be(description);
    }

    [Test]
    public void Should_create_a_notification_with_correct_created_at()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        // Act
        var notification = new Notification(institutionId, title, description);

        // Assert
        notification.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Should_convert_notification_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "Boas-vindas!";
        const string description = "Seja muito bem-vindo(a)!";

        var notification = new Notification(institutionId, title, description)
        {
            Views = "2/15"
        };

        // Act
        var notificationOut = notification.ToOut();

        // Assert
        notificationOut.Id.Should().Be(notification.Id);
        notificationOut.Title.Should().Be(notification.Title);
        notificationOut.Description.Should().Be(notification.Description);
        notificationOut.CreatedAt.Should().Be(notification.CreatedAt);
        notificationOut.Views.Should().Be(notification.Views);
    }
}
