using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterUnitTests
{
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
