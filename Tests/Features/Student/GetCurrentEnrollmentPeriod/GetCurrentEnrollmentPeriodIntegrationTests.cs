namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_current_enrollment_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        await client.CreateEnrollmentPeriod(period.Id);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_not_exists()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.IsEmpty().Should().BeTrue();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_not_started_yet()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        await client.CreateEnrollmentPeriod(period.Id, 2, 4);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_get_empty_current_enrollment_period_when_already_ended()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var data = await client.CreateBasicInstitutionData();
        var period = data.AcademicPeriod2;

        await client.CreateEnrollmentPeriod(period.Id, -4, -2);

        StudentOut student = await client.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var currentEnrollmentPeriod = await studentClient.GetCurrentEnrollmentPeriod();

        // Assert
        currentEnrollmentPeriod.Id.Should().NotBeEmpty();
    }
}
