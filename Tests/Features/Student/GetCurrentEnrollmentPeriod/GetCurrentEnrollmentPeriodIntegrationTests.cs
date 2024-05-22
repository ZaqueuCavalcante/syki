namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_current_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("ADS");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_not_exists()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("ADS");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.IsEmpty().Should().BeTrue();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_not_started_yet()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("ADS");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(2));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(4));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_already_ended()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var course = await client.CreateCourse("ADS");
        var cc = await client.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await client.CreateCourseOffering(campus.Id, course.Id, cc.Id, period.Id, Shift.Noturno);

        var start = DateOnly.FromDateTime(DateTime.Now.AddDays(-4));
        var end = DateOnly.FromDateTime(DateTime.Now.AddDays(-2));
        await client.CreateEnrollmentPeriod(period.Id, start, end);

        var student = await client.CreateStudent(courseOffering.Id, "Zaqueu");
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }
}
