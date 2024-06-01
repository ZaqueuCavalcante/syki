namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_seller_course_offerings()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();

        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var cc = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        await academicClient.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        // Act
        var courseOfferings = await academicClient.GetCourseOfferings();

        // Assert
        courseOfferings.Count.Should().Be(1);
    }
}
