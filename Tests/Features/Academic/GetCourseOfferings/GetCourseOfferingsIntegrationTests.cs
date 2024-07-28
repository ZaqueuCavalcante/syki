namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_course_offerings()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var (cc, _) = await client.CreateCourseCurriculumTuple("Grade de ADS 1.0", course.Id);
        await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        // Act
        var courseOfferings = await client.GetCourseOfferings();

        // Assert
        courseOfferings.Count.Should().Be(1);
    }
}
