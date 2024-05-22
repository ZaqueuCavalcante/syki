namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_view_user_notifications()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);
        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        await client.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        await studentClient.ViewNotifications();

        // Assert
        var response = await studentClient.GetUserNotifications();
        response.Count.Should().Be(1);
        response[0].ViewedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}
