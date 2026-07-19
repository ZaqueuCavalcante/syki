namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_not_get_notification_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetInstitutionNotification(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_not_get_notification_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetInstitutionNotification(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_not_get_notification_when_it_does_not_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionNotification(159);

        // Assert
        result.ShouldBeError(NotificationNotFound.I);
    }

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_not_get_notification_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var notification = (await director.CreateNotification()).Success;

        var other = await _back.LoggedAsDirector();

        // Act
        var result = await other.GetInstitutionNotification(notification.Id);

        // Assert
        result.ShouldBeError(NotificationNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_get_the_notification()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, teacherEmail);
        var created = (await director.CreateNotification("Aviso importante", "Descrição do aviso.", UsersGroup.Teachers)).Success;

        // Act
        var result = await director.GetInstitutionNotification(created.Id);

        // Assert
        var notification = result.Success;
        notification.Id.Should().Be(created.Id);
        notification.Title.Should().Be("Aviso importante");
        notification.Description.Should().Be("Descrição do aviso.");
        notification.Recipients.Should().Be(1);
        notification.Viewed.Should().Be(0);
        notification.ViewRate.Should().Be(0M);
        notification.ViewsByDay.Should().BeEmpty();
    }

    [Test]
    public async Task Notifications_GetInstitutionNotification_Should_get_the_notification_views_grouped_by_day()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, teacherEmail);
        var created = (await director.CreateNotification(targetUsers: UsersGroup.Teachers)).Success;

        var teacher = await _back.LoginAs(teacherEmail);
        await teacher.MarkNotificationsAsViewed(markAll: true);

        // Act
        var result = await director.GetInstitutionNotification(created.Id);

        // Assert
        var notification = result.Success;
        notification.Recipients.Should().Be(1);
        notification.Viewed.Should().Be(1);
        notification.ViewRate.Should().Be(100M);
        notification.ViewsByDay.Should().HaveCount(1);
        notification.ViewsByDay[0].Views.Should().Be(1);
    }

    #endregion
}
