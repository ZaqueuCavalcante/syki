namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_academic_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateCampus();
        await client.CreateDiscipline();
        await client.CreateCourse();

        await client.CreateTeacher();
        await client.CreateTeacher();

        // Act
        var response = await client.GetAcademicInsights();

        // Assert
        response.Campus.Should().Be(1);
        response.Courses.Should().Be(1);
        response.Disciplines.Should().Be(1);
        response.CourseCurriculums.Should().Be(0);
        response.CourseOfferings.Should().Be(0);
        response.Classes.Should().Be(0);
        response.Teachers.Should().Be(2);
        response.Students.Should().Be(0);
        response.Notifications.Should().Be(0);
    }
}
