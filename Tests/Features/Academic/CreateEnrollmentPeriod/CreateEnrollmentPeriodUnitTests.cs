using Syki.Back.Features.Academic.CreateEnrollmentPeriod;

namespace Syki.Tests.Features.Academic.CreateEnrollmentPeriod;

public class CreateEnrollmentPeriodUnitTests
{
    [Test]
    public void Should_create_enrollment_period_with_correct_data()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.CreateVersion7();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 06, 01);

        // Act
        var period = new EnrollmentPeriod(id, institutionId, start, end);

        // Assert
        period.Id.Should().Be(id);
        period.InstitutionId.Should().Be(institutionId);
        period.StartAt.Should().Be(start);
        period.EndAt.Should().Be(end);
    }

    [Test]
    public void Should_not_create_enrollment_period_with_start_equal_to_end()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.CreateVersion7();
        var start = new DateOnly(2023, 02, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        var result = EnrollmentPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }

    [Test]
    public void Should_not_create_enrollment_period_with_start_after_end()
    {
        // Arrange
        const string id = "2023.1";
        var institutionId = Guid.CreateVersion7();
        var start = new DateOnly(2023, 06, 01);
        var end = new DateOnly(2023, 02, 01);

        // Act
        var result = EnrollmentPeriod.New(id, institutionId, start, end);

        // Assert
        result.ShouldBeError(new InvalidEnrollmentPeriodDates());
    }
}
