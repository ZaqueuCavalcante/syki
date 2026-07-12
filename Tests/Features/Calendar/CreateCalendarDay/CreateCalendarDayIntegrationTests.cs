namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_create_day_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateCalendarDay();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_create_day_when_user_is_not_a_manager()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateCalendarDay();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_create_day_with_invalid_type()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCalendarDay(dayType: null);

        // Assert
        result.ShouldBeError(InvalidCalendarDayType.I);
    }

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_create_day_with_too_long_description()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCalendarDay(description: new string('a', 101));

        // Assert
        result.ShouldBeError(InvalidCalendarDayDescription.I);
    }

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_create_day_that_already_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var date = new DateTime(2026, 2, 10);
        await client.CreateCalendarDay(date);

        // Act
        var result = await client.CreateCalendarDay(date);

        // Assert
        result.ShouldBeError(CalendarDayAlreadyExists.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_create_the_day()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCalendarDay(new DateTime(2026, 3, 16), DayType.Recess, "Semana de provas");

        // Assert
        result.Success.Id.Should().BePositive();

        var calendar = (await client.GetInstitutionCalendar(2026)).Success;
        var day = calendar.Items.First(x => x.Date == new DateTime(2026, 3, 16));
        day.Id.Should().Be(result.Success.Id);
        day.DayType.Should().Be(DayType.Recess);
        day.Description.Should().Be("Semana de provas");
    }

    [Test]
    public async Task Calendar_CreateCalendarDay_Should_not_see_days_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        await director.CreateCalendarDay(new DateTime(2026, 4, 15), DayType.Vacation, "Férias");

        var other = await _back.LoggedAsDirector();

        // Act
        var calendar = (await other.GetInstitutionCalendar(2026)).Success;

        // Assert
        var day = calendar.Items.First(x => x.Date == new DateTime(2026, 4, 15));
        day.Id.Should().BeNull();
        day.DayType.Should().Be(DayType.Default);
    }

    #endregion
}
