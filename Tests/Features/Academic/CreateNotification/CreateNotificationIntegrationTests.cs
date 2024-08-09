namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_notification_for_all_users()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateTeacher("Chico");
        await client.CreateStudent(data.CourseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        // Assert
        await using var ctx = _back.GetDbContext();

        var notification = await ctx.Notifications.FirstAsync(x => x.Id == response.Id);
        notification.Title.Should().Be("Hello");
        notification.Description.Should().Be("Hi");

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.NotificationId == notification.Id)
            .ToListAsync();

        userNotifications.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_create_notification_only_for_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        TeacherOut teacher = await client.CreateTeacher("Chico");
        await client.CreateStudent(data.CourseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        // Assert
        await using var ctx = _back.GetDbContext();

        var notification = await ctx.Notifications.FirstAsync(x => x.Id == response.Id);
        notification.Title.Should().Be("Hello");
        notification.Description.Should().Be("Hi");

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.NotificationId == notification.Id)
            .ToListAsync();

        userNotifications.Should().HaveCount(1);
        userNotifications[0].UserId.Should().Be(teacher.Id);
    }

    [Test]
    public async Task Should_create_a_notification_only_for_students()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateTeacher("Chico");
        StudentOut student = await client.CreateStudent(data.CourseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        // Assert
        await using var ctx = _back.GetDbContext();

        var notification = await ctx.Notifications.FirstAsync(x => x.Id == response.Id);
        notification.Title.Should().Be("Hello");
        notification.Description.Should().Be("Hi");

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.NotificationId == notification.Id)
            .ToListAsync();

        userNotifications.Should().HaveCount(1);
        userNotifications[0].UserId.Should().Be(student.Id);
    }
}
