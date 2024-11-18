namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_adm_insights()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        await academicClient.CreateTeacher("Chico");
        await academicClient.CreateTeacher("Ana");

        await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Maju");

        var admClient = await _api.LoggedAsAdm();

        // Act
        var insights = await admClient.GetAdmInsights();

        // Assert
        insights.Institutions.Should().BeGreaterThanOrEqualTo(1);
        insights.Users.Should().BeGreaterThanOrEqualTo(5);
        insights.Campi.Should().BeGreaterThanOrEqualTo(1);
        insights.Courses.Should().BeGreaterThanOrEqualTo(1);
        insights.Disciplines.Should().BeGreaterThanOrEqualTo(6);
        insights.CourseCurriculums.Should().BeGreaterThanOrEqualTo(1);
        insights.CourseOfferings.Should().BeGreaterThanOrEqualTo(1);
        insights.Teachers.Should().BeGreaterThanOrEqualTo(2);
        insights.Students.Should().BeGreaterThanOrEqualTo(2);
    }
}
