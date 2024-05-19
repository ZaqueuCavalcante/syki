namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_student_with_course_offering()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.CreateCourse("ADS");
        var courseCurriculum = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        // Act
        var response = await client.CreateStudent(courseOffering.Id);

        // Assert
        response.Id.Should().NotBeEmpty(); 
        response.CourseOfferingId.Should().Be(courseOffering.Id); 
        response.Name.Should().Be("Zezin"); 
    }

    [Test]
    public async Task Should_not_create_student_without_course_offering()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateStudentHttp(Guid.NewGuid());

        // Assert
        await response.AssertBadRequest(Throw.DE012);
    }
}
