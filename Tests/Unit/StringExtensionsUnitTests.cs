using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;

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
}
