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

    [Test]
    [TestCaseSource(nameof(ValidBirthdates))]
    public void DateOnlyExtensions_Should_return_true_when_birthdate_is_valid(DateOnly date)
    {
        // Arrange / Act
        var result = date.IsValidBirthdate();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(InvalidBirthdates))]
    public void DateOnlyExtensions_Should_return_false_when_birthdate_is_invalid(DateOnly date)
    {
        // Arrange / Act
        var result = date.IsValidBirthdate();

        // Assert
        result.Should().BeFalse();
    }

    private static IEnumerable<object[]> ValidBirthdates()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        foreach (var day in new List<DateOnly>()
        {
            new(1900, 01, 01), // limite inferior
            new(1950, 06, 15),
            new(1995, 01, 20),
            new(2000, 05, 10),
            today,             // limite superior
            today.AddDays(-1),
        })
        {
            yield return [day];
        }
    }

    private static IEnumerable<object[]> InvalidBirthdates()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        foreach (var day in new List<DateOnly>()
        {
            DateOnly.MinValue,
            new(1899, 12, 31), // antes do limite inferior
            today.AddDays(1),  // no futuro
            today.AddYears(1),
            DateOnly.MaxValue,
        })
        {
            yield return [day];
        }
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
