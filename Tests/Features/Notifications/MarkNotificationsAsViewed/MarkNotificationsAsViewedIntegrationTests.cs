using Estud.Back.Features.Notifications.MarkNotificationsAsViewed;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Notifications_MarkNotificationsAsViewed_Should_not_mark_notifications_as_viewed_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.MarkNotificationsAsViewed(markAll: true);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Notifications_MarkNotificationsAsViewed_Should_not_mark_notifications_as_viewed_without_id_when_not_marking_all()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.MarkNotificationsAsViewed(markAll: false, notificationId: null);

        // Assert
        result.ShouldBeError(InvalidNotificationId.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Notifications_MarkNotificationsAsViewed_Should_mark_a_single_notification_as_viewed()
    {
        // Arrange
        var manager = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await manager.CreateTeacher(DataGen.UserName, teacherEmail);
        var notification = await manager.CreateNotification(targetUsers: UsersGroup.Teachers).Success();

        var teacher = await _back.LoginAs(teacherEmail);

        // Act
        var result = await teacher.MarkNotificationsAsViewed(notificationId: notification.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var unreadCount = await teacher.GetUnreadNotificationsCount().Success();
        unreadCount.Count.Should().Be(0);
    }

    [Test]
    public async Task Notifications_MarkNotificationsAsViewed_Should_mark_all_notifications_as_viewed()
    {
        // Arrange
        var manager = await _back.LoggedAsDirector();
        var teacherEmail = DataGen.Email;
        await manager.CreateTeacher(DataGen.UserName, teacherEmail);
        await manager.CreateNotification(targetUsers: UsersGroup.Teachers);
        await manager.CreateNotification(targetUsers: UsersGroup.Teachers);

        var teacher = await _back.LoginAs(teacherEmail);

        // Act
        var result = await teacher.MarkNotificationsAsViewed(markAll: true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var unreadCount = await teacher.GetUnreadNotificationsCount().Success();
        unreadCount.Count.Should().Be(0);
    }

    #endregion
}
