using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterUnitTests
{
    [Test]
    public void Should_create_a_user_register_with_id()
    {
        // Arrange
        const string email = "zaqueu@syki.com";

        // Act
        var register = new UserRegister(email);

        // Assert
        register.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_user_register_with_correct_email()
    {
        // Arrange
        const string email = "zaqueu@syki.com";

        // Act
        var register = new UserRegister(email);

        // Assert
        register.Email.Should().Be(email);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidEmails))]
    public void Should_not_create_a_user_register_with_invalid_email(string email)
    {
        // Arrange // Act
        Action act = () => new UserRegister(email);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(Throw.DE016);
    }
}
