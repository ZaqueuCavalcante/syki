namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_update_enrollment_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        EnrollmentPeriodOut enrollmentPeriod = await client.CreateEnrollmentPeriod(period.Id, -1, 1);

        // Act
        EnrollmentPeriodOut response = await client.UpdateEnrollmentPeriod(period.Id, -5, 6);

        // Assert
        response.Id.Should().Be(enrollmentPeriod.Id);
        response.StartAt.Should().Be(enrollmentPeriod.StartAt.AddDays(-4));
        response.EndAt.Should().Be(enrollmentPeriod.EndAt.AddDays(5));
    }

    [Test]
    public async Task Should_not_update_enrollment_period_without_academic_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        
        // Act
        var response = await client.UpdateEnrollmentPeriod("2024.1");

        // Assert
        response.ShouldBeError(AcademicPeriodNotFound.I);
    }

    [Test]
    public async Task Should_not_update_enrollment_period_when_not_found()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.UpdateEnrollmentPeriod(period.Id);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodNotFound());
    }

    [Test]
    public async Task Should_not_update_enrollment_period_with_start_equal_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id);

        // Act
        var response = await client.UpdateEnrollmentPeriod(period.Id, 0, 0);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }

    [Test]
    public async Task Should_not_update_enrollment_period_with_start_greater_than_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id);

        // Act
        var response = await client.UpdateEnrollmentPeriod(period.Id, 2, -2);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }
}
