namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Periods_GetAcademicPeriods_Should_return_401_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetAcademicPeriods();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Periods_GetAcademicPeriods_Should_not_get_academic_periods_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetAcademicPeriods();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Periods_GetAcademicPeriods_Should_return_empty_list_when_there_are_no_academic_periods()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetAcademicPeriods();

        // Assert
        var periods = result.Success;
        periods.Total.Should().Be(0);
        periods.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Periods_GetAcademicPeriods_Should_get_academic_periods()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateAcademicPeriod("2024.1", new DateOnly(2024, 02, 01), new DateOnly(2024, 06, 01));
        await client.CreateAcademicPeriod("2024.2", new DateOnly(2024, 08, 01), new DateOnly(2024, 12, 01));

        // Act
        var result = await client.GetAcademicPeriods();

        // Assert
        var periods = result.Success;
        periods.Total.Should().Be(2);
        periods.Items.First().Name.Should().Be("2024.1");
        periods.Items.Last().Name.Should().Be("2024.2");
    }

    #endregion
}
