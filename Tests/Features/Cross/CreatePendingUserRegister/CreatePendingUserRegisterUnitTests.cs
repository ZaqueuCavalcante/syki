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
        var institution = new UserRegister(email);

        // Assert
        institution.Id.Should().NotBeEmpty();
        institution.Email.Should().Be(email);
        institution.TrialStart.Should().BeNull();
        institution.TrialEnd.Should().BeNull();
    }
}
