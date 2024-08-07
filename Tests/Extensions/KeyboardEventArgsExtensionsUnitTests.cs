using Syki.Front.Extensions;
using Microsoft.AspNetCore.Components.Web;

namespace Syki.Tests.Extensions;

public class KeyboardEventArgsExtensionsUnitTests
{
    [Test]
    public void Should_return_true_when_enter_key_is_pressed()
    {
        // Arrange
        var args = new KeyboardEventArgs() { Key = "Enter" };
        
        // Act
        var result = args.IsEnter();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_return_false_when_enter_key_is_not_pressed()
    {
        // Arrange
        var args = new KeyboardEventArgs() { Key = "Space" };
        
        // Act
        var result = args.IsEnter();

        // Assert
        result.Should().BeFalse();
    }
}
