namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_academic_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        const string id = "2024.1";
        var start = new DateOnly(2024, 02, 01);
        var end = new DateOnly(2024, 06, 01);

        // Act
        AcademicPeriodOut period = await client.CreateAcademicPeriod(id, start, end);

        // Assert
        period.Id.Should().Be(id);
        period.StartAt.Should().Be(start);
        period.EndAt.Should().Be(end);
    }

    [Test]
    public async Task Should_not_create_duplicated_academic_period()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2024.1");
        var response = await client.CreateAcademicPeriod("2024.1");

        // Assert
        response.ShouldBeError(new AcademicPeriodAlreadyExists());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPeriods))]
    public async Task Should_not_create_academic_period_with_invalid_id(string id)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateAcademicPeriod(id);

        // Assert
        response.ShouldBeError(new InvalidAcademicPeriod());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_invalid_start()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var start = new DateOnly(2022, 02, 01);
        var end = new DateOnly(2023, 06, 01);
        
        // Act
        var response = await client.CreateAcademicPeriod("2023.1", start, end);

        // Assert
        response.ShouldBeError(new InvalidAcademicPeriodStartDate());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_invalid_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2024, 06, 01);
        
        // Act
        var response = await client.CreateAcademicPeriod("2023.1", start, end);

        // Assert
        response.ShouldBeError(new InvalidAcademicPeriodEndDate());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_start_equal_to_end()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);
        
        // Act
        var response = await client.CreateAcademicPeriod("2023.1", start, end);

        // Assert
        response.ShouldBeError(new InvalidAcademicPeriodDates());
    }
}
