using Syki.Front.Components.Passwords;

namespace Syki.Tests.Components;

public class SetupPasswordUnitTests
{
    [Test]
    public void Should_return_false_when_password_has_no_numbers()
    {
        // Arrange
        var setup = new SetupPassword { Password = "Test@test" };

        // Act
        setup.Validate();

        // Assert
        setup.Validation.HasNumbers.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_password_has_no_lower()
    {
        // Arrange
        var setup = new SetupPassword { Password = "TEST@123" };

        // Act
        setup.Validate();

        // Assert
        setup.Validation.HasLower.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_password_has_no_upper()
    {
        // Arrange
        var setup = new SetupPassword { Password = "test@123" };

        // Act
        setup.Validate();

        // Assert
        setup.Validation.HasUpper.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_password_has_no_length()
    {
        // Arrange
        var setup = new SetupPassword { Password = "Test@12" };

        // Act
        setup.Validate();

        // Assert
        setup.Validation.HasLength.Should().BeFalse();
    }

    [Test]
    public void Should_return_false_when_password_has_no_non_alphanumeric()
    {
        // Arrange
        var setup = new SetupPassword { Password = "Test1234" };

        // Act
        setup.Validate();

        // Assert
        setup.Validation.HasNonAlphanumeric.Should().BeFalse();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public void Should_return_false_when_password_is_invalid(string password)
    {
        // Arrange
        var setup = new SetupPassword { Password = password };

        // Act
        setup.Validate();

        // Assert
        setup.IsValid().Should().BeFalse();
    }
}
