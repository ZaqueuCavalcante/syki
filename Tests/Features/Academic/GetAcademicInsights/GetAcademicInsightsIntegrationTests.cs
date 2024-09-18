namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_academic_insights()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        await client.CreateTeacher("Chico Bento");
        await client.CreateTeacher("Ana");

        await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        await client.CreateStudent(data.AdsCourseOffering.Id, "Maju");
        await client.CreateStudent(data.AdsCourseOffering.Id, "Zezinho");

        // Act
        var response = await client.GetAcademicInsights();

        // Assert
        response.Campus.Should().Be(1);
        response.Courses.Should().Be(2);
        response.Disciplines.Should().Be(17);
        response.CourseCurriculums.Should().Be(2);
        response.CourseOfferings.Should().Be(2);
        response.Classes.Should().Be(0);
        response.Teachers.Should().Be(2);
        response.Students.Should().Be(3);
        response.Notifications.Should().Be(0);
    }
}
