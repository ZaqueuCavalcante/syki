namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_notification_for_all_users()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await client.CreateTeacher("Chico");
        await client.CreateStudent(courseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        // Assert
        using var ctx = _back.GetDbContext();

        var notification = await ctx.Notifications.FirstAsync(x => x.Id == response.Id);
        notification.Title.Should().Be("Hello");
        notification.Description.Should().Be("Hi");

        var userNotifications = await ctx.UserNotifications
            .Where(x => x.NotificationId == notification.Id)
            .ToListAsync();
        userNotifications.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_create_a_notification_only_for_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var teacher = await client.CreateTeacher("Chico");
        await client.CreateStudent(courseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        // Assert
        using var ctx = _back.GetDbContext();

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

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await client.CreateTeacher("Chico");
        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        // Assert
        using var ctx = _back.GetDbContext();

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
