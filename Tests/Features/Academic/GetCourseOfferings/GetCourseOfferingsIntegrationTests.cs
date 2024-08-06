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
        var courseCurriculum = (await client.CreateCourseCurriculum2("Grade de ADS 1.0", course.Id)).GetSuccess();
        await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        // Act
        var courseOfferings = await client.GetCourseOfferings();

        // Assert
        courseOfferings.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_not_return_any_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var courseOfferings = await client.GetCourseOfferings();

        // Assert
        courseOfferings.Should().BeEmpty();
    }
}
