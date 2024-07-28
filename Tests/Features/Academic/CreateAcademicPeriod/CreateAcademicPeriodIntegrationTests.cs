namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var period = await client.CreateAcademicPeriod("2024.1");

        // Assert
        period.Id.Should().Be(period.Id);
    }

    [Test]
    public async Task Should_not_create_duplicated_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2024.1");
        var (_, response) = await client.CreateAcademicPeriodTuple("2024.1");

        // Assert
        await response.AssertBadRequest(new AcademicPeriodAlreadyExists());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPeriods))]
    public async Task Should_not_create_academic_period_with_invalid_id(string id)
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var (_, response) = await client.CreateAcademicPeriodTuple(id);

        // Assert
        await response.AssertBadRequest(new InvalidAcademicPeriod());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_invalid_start()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var start = new DateOnly(2022, 02, 01);
        var end = new DateOnly(2023, 06, 01);
        
        // Act
        var (_, response) = await client.CreateAcademicPeriodTuple("2023.1", start, end);

        // Assert
        await response.AssertBadRequest(new InvalidAcademicPeriodStartDate());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_invalid_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2024, 06, 01);
        
        // Act
        var (_, response) = await client.CreateAcademicPeriodTuple("2023.1", start, end);

        // Assert
        await response.AssertBadRequest(new InvalidAcademicPeriodEndDate());
    }

    [Test]
    public async Task Should_not_create_academic_period_with_start_equal_to_end()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);
        
        // Act
        var (_, response) = await client.CreateAcademicPeriodTuple("2023.1", start, end);

        // Assert
        await response.AssertBadRequest(new AcademicPeriodStartDateShouldBeLessThanEndDate());
    }
}
