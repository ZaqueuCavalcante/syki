namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_not_update_day_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateCalendarDay(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_not_update_day_when_user_is_not_a_manager()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateCalendarDay(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_not_update_day_with_invalid_type()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var day = (await client.CreateCalendarDay(new DateTime(2026, 5, 4))).Success;

        // Act
        var result = await client.UpdateCalendarDay(day.Id, dayType: null);

        // Assert
        result.ShouldBeError(InvalidCalendarDayType.I);
    }

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_not_update_day_that_does_not_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateCalendarDay(159);

        // Assert
        result.ShouldBeError(CalendarDayNotFound.I);
    }

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_not_update_day_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var day = (await director.CreateCalendarDay(new DateTime(2026, 5, 5))).Success;

        var other = await _back.LoggedAsDirector();

        // Act
        var result = await other.UpdateCalendarDay(day.Id);

        // Assert
        result.ShouldBeError(CalendarDayNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_update_the_day()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var created = (await client.CreateCalendarDay(new DateTime(2026, 6, 24), DayType.Vacation, "Férias")).Success;

        // Act
        var result = await client.UpdateCalendarDay(created.Id, DayType.Holiday, "São João");

        // Assert
        var day = result.Success;
        day.Id.Should().Be(created.Id);
        day.Date.Should().Be(new DateTime(2026, 6, 24));
        day.DayType.Should().Be(DayType.Holiday);
        day.Description.Should().Be("São João");
    }

    [Test]
    public async Task Calendar_UpdateCalendarDay_Should_reset_the_day_to_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var created = (await client.CreateCalendarDay(new DateTime(2026, 7, 20))).Success;

        // Act
        await client.UpdateCalendarDay(created.Id, DayType.Default, null);

        // Assert
        var calendar = (await client.GetInstitutionCalendar(2026)).Success;
        var day = calendar.Items.First(x => x.Date == new DateTime(2026, 7, 20));
        day.DayType.Should().Be(DayType.Default);
        day.Description.Should().BeNull();
    }

    #endregion
}
