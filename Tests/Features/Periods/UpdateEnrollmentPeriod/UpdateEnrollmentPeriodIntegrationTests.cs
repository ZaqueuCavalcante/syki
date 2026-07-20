namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateEnrollmentPeriod(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateEnrollmentPeriod(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_when_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateEnrollmentPeriod(999999);

        // Assert
        result.ShouldBeError(EnrollmentPeriodNotFound.I);
    }

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_of_another_institution()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.1").Success();

        var otherClient = await _back.LoggedAsDirector();

        // Act
        var result = await otherClient.UpdateEnrollmentPeriod(period.Id);

        // Assert
        result.ShouldBeError(EnrollmentPeriodNotFound.I);
    }

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_with_start_equal_to_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.1").Success();

        // Act
        var result = await client.UpdateEnrollmentPeriod(period.Id, startAt: new DateOnly(2024, 01, 15), endAt: new DateOnly(2024, 01, 15));

        // Assert
        result.ShouldBeError(InvalidEnrollmentPeriodDates.I);
    }

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_with_start_after_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.1").Success();

        // Act
        var result = await client.UpdateEnrollmentPeriod(period.Id, startAt: new DateOnly(2024, 02, 01), endAt: new DateOnly(2024, 01, 15));

        // Assert
        result.ShouldBeError(InvalidEnrollmentPeriodDates.I);
    }

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_not_update_enrollment_period_to_an_existing_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateEnrollmentPeriod("Matrículas 2024.1");
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.2").Success();

        // Act
        var result = await client.UpdateEnrollmentPeriod(period.Id, name: "Matrículas 2024.1");

        // Assert
        result.ShouldBeError(EnrollmentPeriodAlreadyExists.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_update_enrollment_period()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.1").Success();

        // Act
        var result = await client.UpdateEnrollmentPeriod(
            period.Id,
            name: "Matrículas 2024.2",
            startAt: new DateOnly(2024, 07, 01),
            endAt: new DateOnly(2024, 07, 20)
        );

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(period.Id);
        updated.Name.Should().Be("Matrículas 2024.2");
        updated.StartAt.Should().Be(new DateOnly(2024, 07, 01));
        updated.EndAt.Should().Be(new DateOnly(2024, 07, 20));

        await using var db = _back.GetDbContext();
        var entity = await db.EnrollmentPeriods.FirstAsync(p => p.Id == period.Id);
        entity.Name.Should().Be("Matrículas 2024.2");
        entity.StartAt.Should().Be(new DateOnly(2024, 07, 01));
        entity.EndAt.Should().Be(new DateOnly(2024, 07, 20));
    }

    [Test]
    public async Task Periods_UpdateEnrollmentPeriod_Should_update_enrollment_period_keeping_its_own_name()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateEnrollmentPeriod("Matrículas 2024.1").Success();

        // Act
        var result = await client.UpdateEnrollmentPeriod(
            period.Id,
            name: "Matrículas 2024.1",
            startAt: new DateOnly(2024, 07, 01),
            endAt: new DateOnly(2024, 07, 20)
        );

        // Assert
        var updated = result.Success;
        updated.Name.Should().Be("Matrículas 2024.1");
        updated.StartAt.Should().Be(new DateOnly(2024, 07, 01));
    }

    #endregion
}
