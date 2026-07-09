namespace Estud.Tests.Extensions;

public class DateOnlyExtensionsUnitTests
{
    [Test]
    [TestCaseSource(nameof(Holidays))]
    public void DateOnlyExtensions_Should_return_true_when_is_holiday(DateOnly day)
    {
        // Arrange / Act
        var result = day.IsHoliday();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(NotHolidays))]
    public void DateOnlyExtensions_Should_return_false_when_is_not_holiday(DateOnly day)
    {
        // Arrange / Act
        var result = day.IsHoliday();

        // Assert
        result.Should().BeFalse();
    }

    private static IEnumerable<object[]> Holidays()
    {
        foreach (var day in new List<DateOnly>()
        {
            new(2024, 01, 01), // Confraternização Universal
            new(2024, 04, 21), // Tiradentes
            new(2024, 05, 01), // Dia do Trabalho
            new(2024, 09, 07), // Independência do Brasil
            new(2024, 10, 12), // Nossa Senhora Aparecida
            new(2024, 11, 02), // Finados
            new(2024, 11, 15), // Proclamação da República
            new(2024, 12, 25), // Natal
        })
        {
            yield return [day];
        }
    }

    private static IEnumerable<object[]> NotHolidays()
    {
        foreach (var day in new List<DateOnly>()
        {
            new(2024, 01, 02),
        })
        {
            yield return [day];
        }
    }
}
