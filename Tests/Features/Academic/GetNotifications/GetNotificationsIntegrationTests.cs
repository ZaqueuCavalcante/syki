namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_academic_notifications()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        await client.CreateNotification("Hello", "Hi", UsersGroup.Students, true);
        await client.CreateNotification("Olá", "Olá", UsersGroup.Teachers, true);

        // Act
        var notifications = await client.GetNotifications();

        // Assert
        notifications.Count.Should().Be(2);
    }

    [Test]
    public async Task Should_return_empty_notifications()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var notifications = await client.GetNotifications();

        // Assert
        notifications.Should().BeEmpty();
    }
}
