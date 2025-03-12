using Bogus;

namespace Syki.Tests.Extensions;

public class StringExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.CamelCaseNames))]
    public void Should_change_to_snake_case(string camel, string snake)
    {
        // Arrange / Act
        var result = camel.ToSnakeCase();

        // Assert
        result.Should().Be(snake);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.FormatedStrings))]
    public void Should_change_to_only_numbers(string text, string numbers)
    {
        // Arrange / Act
        var result = text.OnlyNumbers();

        // Assert
        result.Should().Be(numbers);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.TextsContains))]
    public void Should_return_true_because_serch_is_inside_some_text(string text1, string text2, string? search)
    {
        // Arrange / Act
        var result = search.IsIn(text1, text2);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.TextsNotContains))]
    public void Should_return_false_because_serch_is_not_inside_some_text(string text1, string text2, string search)
    {
        // Arrange / Act
        var result = search.IsIn(text1, text2);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.DecimalsStringsForFormat))]
    public void Should_format_decimal_as_string(decimal number, string text)
    {
        // Arrange / Act
        var result = number.Format();

        // Assert
        result.Should().Be(text);
    }

    [Test]
    [Repeat(100)]
    public void Should_return_true_when_email_is_valid()
    {
        // Arrange
        var email = new Faker().Internet.Email();

        // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidEmails))]
    public void Should_return_false_when_email_is_invalid(string email)
    {
        // Arrange // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.MinutesForFormat))]
    public void Should_format_minutes_as_string(int minutes, string text)
    {
        // Arrange / Act
        var result = minutes.MinutesToString();

        // Assert
        result.Should().Be(text);
    }
}
