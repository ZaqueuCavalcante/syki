namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Notifications_CreateNotification_Should_not_create_notification_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateNotification();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Notifications_CreateNotification_Should_not_create_notification_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateNotification();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Notifications_CreateNotification_Should_not_create_notification_with_invalid_title(string title)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateNotification(title: title);

        // Assert
        result.ShouldBeError(InvalidNotificationTitle.I);
    }

    [Test]
    [TestCase("")]
    public async Task Notifications_CreateNotification_Should_not_create_notification_with_invalid_description(string description)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateNotification(description: description);

        // Assert
        result.ShouldBeError(InvalidNotificationDescription.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Notifications_CreateNotification_Should_create_notification_for_all_users()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateTeacher(DataGen.UserName, DataGen.Email);
        await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Act
        var result = await client.CreateNotification(targetUsers: UsersGroup.All);

        // Assert
        var notification = result.Success;
        notification.Id.Should().NotBe(0);

        await using var ctx = _back.GetDbContext();
        var recipients = await ctx.UserNotifications.CountAsync(x => x.NotificationId == notification.Id);
        recipients.Should().Be(2);
    }

    [Test]
    public async Task Notifications_CreateNotification_Should_create_notification_for_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateTeacher(DataGen.UserName, DataGen.Email);
        await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Act
        var result = await client.CreateNotification(targetUsers: UsersGroup.Students);

        // Assert
        var notification = result.Success;
        notification.Id.Should().NotBe(0);

        await using var ctx = _back.GetDbContext();
        var recipients = await ctx.UserNotifications.CountAsync(x => x.NotificationId == notification.Id);
        recipients.Should().Be(1);
    }

    [Test]
    public async Task Notifications_CreateNotification_Should_create_notification_for_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateTeacher(DataGen.UserName, DataGen.Email);
        await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Act
        var result = await client.CreateNotification(targetUsers: UsersGroup.Teachers);

        // Assert
        var notification = result.Success;
        notification.Id.Should().NotBe(0);

        await using var ctx = _back.GetDbContext();
        var recipients = await ctx.UserNotifications.CountAsync(x => x.NotificationId == notification.Id);
        recipients.Should().Be(1);
    }

    #endregion
}
