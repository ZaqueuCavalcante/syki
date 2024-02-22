using Syki.Back.Exceptions;
using Syki.Back.CreatePendingDemo;

namespace Syki.Tests.CreatePendingDemo;

public class CreatePendingDemoUnitTests
{
    [Test]
    public void Should_create_a_demo_with_not_empty_id()
    {
        // Arrange
        const string email = "demail@syki.com";

        // Act
        var demo = new Demo(email);

        // Assert
        demo.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_demo_with_correct_id()
    {
        // Arrange
        const string email = "demail@syki.com";

        // Act
        var demo = new Demo(email);

        // Assert
        demo.Email.Should().Be(email);
    }

    [Test]
    public void Should_not_create_a_demo_with_invalid_email()
    {
        // Arrange
        const string email = "demailsyki.com";

        // Act
        Action act = () => new Demo(email);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE016);
    }
}
