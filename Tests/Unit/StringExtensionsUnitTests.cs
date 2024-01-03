using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Bogus;

namespace Syki.Tests.Unit;

public class StringExtensionsUnitTests
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.CamelCaseNames))]
    public void Shoud_change_to_snake_case((string camel, string snake) data)
    {
        // Arrange / Act
        var result = data.camel.ToSnakeCase();

        // Assert
        result.Should().Be(data.snake);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.FormatedStrings))]
    public void Shoud_change_to_only_numbers((string text, string numbers) data)
    {
        // Arrange / Act
        var result = data.text.OnlyNumbers();

        // Assert
        result.Should().Be(data.numbers);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.TextsContains))]
    public void Shoud_return_true_because_serch_is_inside_some_text((string text1, string text2, string? search) data)
    {
        // Arrange / Act
        var result = data.search.IsIn(data.text1, data.text2);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.TextsNotContains))]
    public void Shoud_return_false_because_serch_is_not_inside_some_text((string text1, string text2, string search) data)
    {
        // Arrange / Act
        var result = data.search.IsIn(data.text1, data.text2);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.DecimalsStringsForFormat))]
    public void Shoud_format_decimal_as_string((decimal number, string text) data)
    {
        // Arrange / Act
        var result = data.number.Format();

        // Assert
        result.Should().Be(data.text);
    }

    [Test]
    [Repeat(100)]
    public void Shoud_return_true_when_email_is_valid()
    {
        // Arrange
        var email = new Faker().Internet.Email();

        // Act
        var result = email.IsValidEmail();

        // Assert
        result.Should().BeTrue();
    }
}
