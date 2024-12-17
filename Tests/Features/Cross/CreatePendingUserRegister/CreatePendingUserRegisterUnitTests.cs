using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Tests.Features.Cross.CreatePendingUserRegister;

public class CreatePendingUserRegisterUnitTests
{
    [Test]
    public void Should_create_user_register_with_correct_data()
    {
        // Arrange
        var email = TestData.Email;

        // Act
        var register = new UserRegister(email);

        // Assert
        register.Id.Should().NotBeEmpty();
        register.Email.Should().Be(email);
        register.Status.Should().Be(UserRegisterStatus.Pending);
    }

    [Test]
    public void Should_create_user_register_with_domain_event()
    {
        // Arrange
        var email = TestData.Email;

        // Act
        var register = new UserRegister(email);

        // Assert
        var domainEvent = register.ShouldPublishDomainEvent<UserRegisterCreatedDomainEvent>();
        domainEvent.Id.Should().Be(register.Id);
    }
}
