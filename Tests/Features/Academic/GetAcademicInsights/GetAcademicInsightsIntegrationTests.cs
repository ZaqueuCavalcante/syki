namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_academic_insights()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        await client.CreateCampus();
        await client.CreateDiscipline();
        await client.CreateCurso();

        // Act
        var response = await client.GetAcademicInsights();

        // Assert
        response.Campus.Should().Be(1);
        response.Courses.Should().Be(1);
        response.Disciplines.Should().Be(1);
        response.CourseCurriculums.Should().Be(0);
        response.CourseOfferings.Should().Be(0);
        response.Classes.Should().Be(0);
        response.Teachers.Should().Be(0);
        response.Students.Should().Be(0);
        response.Notifications.Should().Be(0);
    }
}
