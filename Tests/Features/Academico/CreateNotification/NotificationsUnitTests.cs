using Syki.Back.CreateNotification;

namespace Syki.Tests.Unit;

public class NotificationsUnitTests
{
    [Test]
    public void Deve_criar_uma_notification_com_id()
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
    public void Deve_criar_uma_notification_com_institution_id_correto()
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
    public void Deve_criar_uma_notification_com_title_correto()
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
    public void Deve_criar_uma_notification_com_description_correta()
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
    public void Deve_criar_uma_notification_com_created_at_correta()
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
    public void Deve_converter_a_notification_corretamente_pro_out()
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
