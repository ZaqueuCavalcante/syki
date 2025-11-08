namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_send_reset_password_token()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;

        // Act
        await _back.GetClient().SendResetPasswordToken(user.Email);

        // Assert
        var token = await _back.GetResetPasswordToken(user.Email);
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Should_not_send_a_reset_password_email_when_user_not_exists()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;

        // Act
        var response = await _back.GetClient().SendResetPasswordToken(user.Email + "a");

        // Assert
        await response.AssertBadRequest(UserNotFound.I);
    }

    [Test]
    public async Task Should_not_get_the_reset_password_token_when_user_not_exists()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;

        // Act
        var token = await _back.GetResetPasswordToken(user.Email + "a");

        // Assert
        token.Should().BeNull();
    }
}
