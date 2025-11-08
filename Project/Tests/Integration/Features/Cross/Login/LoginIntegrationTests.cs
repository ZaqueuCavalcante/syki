namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_login_random_user()
    {
        // Arrange
        var client = _back.GetClient();

        // Act
        var result = await client.Login("user@notfound.com", "User@123");

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_email()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        // Act
        var result = await _back.GetClient().Login(client.User.Email + "1", ExatoCSPassword);

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_password()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        // Act
        var result = await _back.GetClient().Login(client.User.Email, ExatoCSPassword + "1");

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task Should_not_login_user_with_correct_email_and_password_but_needs_mfa()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();
        var user = client.User;

        var keyResponse = await client.Cross.GetTwoFactorAuthenticationKey();
        var token = keyResponse.Key.GenerateTOTP();
        await client.Cross.SetupTwoFactorAuthentication(token);

        await client.Cross.Logout();

        // Act
        var result = await client.Cross.Login(user.Email, ExatoCSPassword);

        // Assert
        result.ShouldBeError(LoginRequiresTwoFactor.I);
    }
}
