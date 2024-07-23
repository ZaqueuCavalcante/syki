namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var enrollmentPeriod = await client.CreateEnrollmentPeriod(period.Id);

        // Assert
        enrollmentPeriod.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_start_greater_than_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriodHttp(period.Id, "15/01", "03/01");

        // Assert
        await response.AssertBadRequest(new EnrollmentPeriodStartDateShouldBeLessThanEndDate());
    }
}
