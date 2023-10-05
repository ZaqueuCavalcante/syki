using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;

namespace Syki.Tests.Unit;

public class SykiExtensionsUnitTests
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
}
