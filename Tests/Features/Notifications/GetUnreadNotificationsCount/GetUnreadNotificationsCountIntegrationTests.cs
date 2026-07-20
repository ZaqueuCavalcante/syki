namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Notifications_GetUnreadNotificationsCount_Should_not_get_unread_count_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetUnreadNotificationsCount();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Notifications_GetUnreadNotificationsCount_Should_get_zero_when_user_has_no_notifications()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetUnreadNotificationsCount().Success();

        // Assert
        result.Count.Should().Be(0);
    }

    [Test]
    public async Task Notifications_GetUnreadNotificationsCount_Should_get_the_unread_notifications_count()
    {
        // Arrange
        var manager = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await manager.CreateTeacher(DataGen.UserName, teacherEmail);
        await manager.CreateNotification(targetUsers: UsersGroup.Teachers);

        var teacher = await _back.LoginAs(teacherEmail);

        // Act
        var result = await teacher.GetUnreadNotificationsCount();

        // Assert
        result.Success.Count.Should().Be(1);
    }

    #endregion
}
