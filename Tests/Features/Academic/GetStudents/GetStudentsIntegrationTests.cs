namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_students()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var co = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        await client.CreateStudent(co.Id, "Zaqueu");
        await client.CreateStudent(co.Id, "Maju");

        // Act
        var response = await client.GetStudents();

        // Assert
        response.Count.Should().Be(2); 
    }
}
