namespace Syki.Tests.Extensions;

public class DateOnlyExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.Holidays))]
    public void Should_return_true_when_is_holiday(DateOnly day)
    {
        // Arrange / Act
        var result = day.IsHoliday();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.NotHolidays))]
    public void Should_return_false_when_is_not_holiday(DateOnly day)
    {
        // Arrange / Act
        var result = day.IsHoliday();

        // Assert
        result.Should().BeFalse();
    }
}
