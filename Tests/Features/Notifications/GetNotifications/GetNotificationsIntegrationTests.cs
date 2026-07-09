namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Notifications_GetNotifications_Should_not_get_notifications_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetNotifications();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Notifications_GetNotifications_Should_get_empty_list_when_user_has_no_notifications()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetNotifications();

        // Assert
        var notifications = result.Success;
        notifications.Total.Should().Be(0);
        notifications.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Notifications_GetNotifications_Should_get_the_user_notifications()
    {
        // Arrange
        var manager = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await manager.CreateTeacher(DataGen.UserName, teacherEmail);
        await manager.CreateNotification("Aviso importante", "Descrição do aviso.", UsersGroup.Teachers);

        var teacher = await _back.LoginAs(teacherEmail);

        // Act
        var result = await teacher.GetNotifications();

        // Assert
        var notifications = result.Success;
        notifications.Total.Should().Be(1);
        notifications.Items.Should().HaveCount(1);
        notifications.Items[0].Title.Should().Be("Aviso importante");
        notifications.Items[0].Description.Should().Be("Descrição do aviso.");
        notifications.Items[0].ViewedAt.Should().BeNull();
    }

    [Test]
    public async Task Notifications_GetNotifications_Should_get_only_unread_notifications_when_unread_only_is_true()
    {
        // Arrange
        var manager = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await manager.CreateTeacher(DataGen.UserName, teacherEmail);
        await manager.CreateNotification(targetUsers: UsersGroup.Teachers);

        var teacher = await _back.LoginAs(teacherEmail);
        await teacher.MarkNotificationsAsViewed(markAll: true);

        // Act
        var result = await teacher.GetNotifications(unreadOnly: true);

        // Assert
        result.Success.Total.Should().Be(0);
        result.Success.Items.Should().BeEmpty();
    }

    #endregion
}
