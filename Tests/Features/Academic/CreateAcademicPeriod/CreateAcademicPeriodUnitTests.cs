using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Tests.Features.Academic.CreateAcademicPeriod;

public class CreateAcademicPeriodUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.ValidPeriods))]
    public void Should_create_academic_period_with_valid_id(string id)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var year = int.Parse(id.Substring(0, 4));
        var start = new DateOnly(year, 02, 01);
        var end = new DateOnly(year, 06, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeSuccess();

        var academicPeriod = result.GetSuccess();
        academicPeriod.Id.Should().Be(id);
        academicPeriod.InstitutionId.Should().Be(institutionId);
        academicPeriod.StartAt.Should().Be(start);
        academicPeriod.EndAt.Should().Be(end);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPeriods))]
    public void Should_not_create_academic_period_with_invalid_id(string id)
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidAcademicPeriod());
    }

    [Test]
    public void Should_not_create_academic_period_with_invalid_start()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2022, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidAcademicPeriodStartDate());
    }

    [Test]
    public void Should_not_create_academic_period_with_invalid_end()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2024, 06, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidAcademicPeriodEndDate());
    }

    [Test]
    public void Should_not_create_academic_period_with_start_equal_to_end()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidAcademicPeriodDates());
    }

    [Test]
    public void Should_not_create_academic_period_with_start_after_end()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2023, 06, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        var result = AcademicPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidAcademicPeriodDates());
    }

    [Test]
    public void Should_convert_academic_period_to_out()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.NewGuid();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        var period = new AcademicPeriod(id, institutionId, start, end);

        // Act
        var periodOut = period.ToOut();

        // Assert
        periodOut.Id.Should().Be(period.Id);
        periodOut.StartAt.Should().Be(start);
        periodOut.EndAt.Should().Be(end);
    }
}
