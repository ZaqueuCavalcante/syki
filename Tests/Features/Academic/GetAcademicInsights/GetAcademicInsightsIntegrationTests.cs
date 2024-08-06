namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_academic_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateBasicInstitutionData();

        // Act
        var response = await client.GetAcademicInsights();

        // Assert
        response.Campus.Should().Be(1);
        response.Courses.Should().Be(1);
        response.Disciplines.Should().Be(6);
        response.CourseCurriculums.Should().Be(1);
        response.CourseOfferings.Should().Be(1);
        response.Classes.Should().Be(0);
        response.Teachers.Should().Be(0);
        response.Students.Should().Be(0);
        response.Notifications.Should().Be(0);
    }
}
