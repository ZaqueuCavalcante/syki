namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_not_create_enrollment_period_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateEnrollmentPeriod();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_not_create_enrollment_period_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateEnrollmentPeriod();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_not_create_enrollment_period_with_start_equal_to_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateEnrollmentPeriod(startAt: new DateOnly(2024, 01, 15), endAt: new DateOnly(2024, 01, 15));

        // Assert
        response.ShouldBeError(InvalidEnrollmentPeriodDates.I);
    }

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_not_create_enrollment_period_with_start_after_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateEnrollmentPeriod(startAt: new DateOnly(2024, 02, 01), endAt: new DateOnly(2024, 01, 15));

        // Assert
        response.ShouldBeError(InvalidEnrollmentPeriodDates.I);
    }

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_not_create_duplicated_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateEnrollmentPeriod("Matrículas 2024.1");

        // Act
        var response = await client.CreateEnrollmentPeriod("Matrículas 2024.1");

        // Assert
        response.ShouldBeError(EnrollmentPeriodAlreadyExists.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Periods_CreateEnrollmentPeriod_Should_create_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateEnrollmentPeriod("Matrículas 2024.1");

        // Assert
        var period = result.Success;
        period.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
