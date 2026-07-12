namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_not_get_calendar_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetInstitutionCalendar();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_not_get_calendar_when_user_is_not_a_manager()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetInstitutionCalendar();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_get_all_days_of_the_year()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2026);

        // Assert
        var calendar = result.Success;
        calendar.Year.Should().Be(2026);
        calendar.Total.Should().Be(365);
        calendar.Items.Should().HaveCount(365);
        calendar.Items[0].Date.Should().Be(new DateTime(2026, 1, 1));
        calendar.Items[^1].Date.Should().Be(new DateTime(2026, 12, 31));
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_get_all_days_of_a_leap_year()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2028);

        // Assert
        result.Success.Total.Should().Be(366);
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_get_the_current_year_when_no_year_is_informed()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar();

        // Assert
        result.Success.Year.Should().Be(DateTime.UtcNow.Year);
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_mark_national_holidays()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2026);

        // Assert
        var calendar = result.Success;

        var christmas = calendar.Items.First(x => x.Date == new DateTime(2026, 12, 25));
        christmas.DayType.Should().Be(DayType.Holiday);
        christmas.Description.Should().Be("Natal");

        // Páscoa de 2026: 05/04. Sexta-feira Santa: 03/04.
        var goodFriday = calendar.Items.First(x => x.Date == new DateTime(2026, 4, 3));
        goodFriday.DayType.Should().Be(DayType.Holiday);
        goodFriday.Description.Should().Be("Sexta-feira Santa");
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_mark_weekends()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2026);

        // Assert
        var calendar = result.Success;

        // 14/03/2026 é um sábado e 15/03/2026 um domingo.
        calendar.Items.First(x => x.Date == new DateTime(2026, 3, 14)).DayType.Should().Be(DayType.Weekend);
        calendar.Items.First(x => x.Date == new DateTime(2026, 3, 15)).DayType.Should().Be(DayType.Weekend);
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_mark_holidays_that_fall_on_a_weekend_as_holidays()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2026);

        // Assert
        // 15/11/2026 (Proclamação da República) cai num domingo: o feriado prevalece sobre o fim de semana.
        var holiday = result.Success.Items.First(x => x.Date == new DateTime(2026, 11, 15));
        holiday.DayType.Should().Be(DayType.Holiday);
        holiday.Description.Should().Be("Proclamação da República");
    }

    [Test]
    public async Task Calendar_GetInstitutionCalendar_Should_mark_common_days_as_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionCalendar(2026);

        // Assert
        var day = result.Success.Items.First(x => x.Date == new DateTime(2026, 3, 10));
        day.DayType.Should().Be(DayType.Default);
        day.Description.Should().BeNull();
    }

    #endregion
}
