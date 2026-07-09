namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateAcademicPeriod();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateAcademicPeriod();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("lalala")]
    [TestCase("1969.1")]
    [TestCase("1970.3")]
    [TestCase("1970.0")]
    [TestCase("1971.90")]
    [TestCase("2001")]
    [TestCase("202")]
    [TestCase("2023.9")]
    [TestCase("2070.0")]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_with_invalid_id(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateAcademicPeriod(name);

        // Assert
        response.ShouldBeError(InvalidAcademicPeriod.I);
    }

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_with_invalid_start()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateAcademicPeriod("2023.1", startAt: new DateOnly(2022, 02, 01));

        // Assert
        response.ShouldBeError(InvalidAcademicPeriodStartDate.I);
    }

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_with_invalid_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateAcademicPeriod("2023.1", endAt: new DateOnly(2024, 06, 01));

        // Assert
        response.ShouldBeError(InvalidAcademicPeriodEndDate.I);
    }

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_academic_period_with_start_equal_to_end()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateAcademicPeriod("2023.1", startAt: new DateOnly(2023, 02, 01), endAt: new DateOnly(2023, 02, 01));

        // Assert
        response.ShouldBeError(InvalidAcademicPeriodDates.I);
    }

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_not_create_duplicated_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateAcademicPeriod("2024.1");

        // Act
        var response = await client.CreateAcademicPeriod("2024.1");

        // Assert
        response.ShouldBeError(AcademicPeriodAlreadyExists.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Periods_CreateAcademicPeriod_Should_create_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateAcademicPeriod("2024.1");

        // Assert
        var period = result.Success;
        period.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
