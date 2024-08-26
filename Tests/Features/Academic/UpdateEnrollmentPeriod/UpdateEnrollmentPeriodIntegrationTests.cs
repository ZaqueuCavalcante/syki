namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_update_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");

        // Act
        EnrollmentPeriodOut response = await client.CreateEnrollmentPeriod(period.Id);

        // Assert
        response.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task Should_not_create_enrollment_period_without_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateEnrollmentPeriod("2024.1", 0, 0);

        // Assert
        response.ShouldBeError(new AcademicPeriodNotFound());
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_start_equal_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriod(period.Id, 0, 0);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_start_greater_than_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriod(period.Id, 2, -2);

        // Assert
        response.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }

    [Test]
    public async Task Should_not_create_enrollment_period_with_duplicated_id()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id);

        // Act
        var response = await client.CreateEnrollmentPeriod(period.Id);

        // Assert
        response.ShouldBeError(new EnrollmentPeriodAlreadyExists());
    }
}
