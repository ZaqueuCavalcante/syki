namespace Syki.Tests.Integration;

public partial class IntegrationTests
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
    public async Task Should_not_create_enrollment_period_without_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateEnrollmentPeriod2("2024.1", 0, 0);

        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_start_equal_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriod2(period.Id, 0, 0);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_start_greater_than_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriod2(period.Id, 2, -2);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }
}
