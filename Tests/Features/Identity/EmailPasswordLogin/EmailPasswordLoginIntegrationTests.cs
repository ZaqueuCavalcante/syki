using Estud.Back.Features.Identity.EmailPasswordLogin;

namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Validation errors

    [Test]
    public async Task EmailPasswordLogin_Should_not_login_random_user()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.EmailPasswordLogin("user@notfound.com", "User@123");

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task EmailPasswordLogin_Should_not_login_user_with_wrong_email()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        // Act
        var result = await client.EmailPasswordLogin(user.Email + "1", "Password");

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task EmailPasswordLogin_Should_not_login_user_with_wrong_password()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        // Act
        var result = await client.EmailPasswordLogin(user.Email, "lalala");

        // Assert
        result.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task EmailPasswordLogin_Should_lockout_user_after_3_failed_login_attempts()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await _back.SetPassword(user.Email, "My@nEw@strong@P4ssword");

        var wrongPassword = "WrongPassword123!";

        // Act
        var attempt1 = await client.EmailPasswordLogin(user.Email, wrongPassword);
        var attempt2 = await client.EmailPasswordLogin(user.Email, wrongPassword);
        var attempt3 = await client.EmailPasswordLogin(user.Email, wrongPassword);
        var attempt4 = await client.EmailPasswordLogin(user.Email, wrongPassword);
        var attempt5 = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert
        attempt1.ShouldBeError(LoginWrongEmailOrPassword.I);
        attempt2.ShouldBeError(LoginWrongEmailOrPassword.I);
        attempt3.ShouldBeError(LoginWrongEmailOrPassword.I);
        attempt4.ShouldBeError(LoginUserLockedOut.I);
        attempt5.ShouldBeError(LoginUserLockedOut.I);
    }

    [Test]
    public async Task EmailPasswordLogin_Should_require_two_factor_when_2fa_is_enabled()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();

        // Act
        var result = await client.EmailPasswordLogin(client.User.Email, "My@nEw@strong@P4ssword");

        // Assert
        result.ShouldBeError(LoginRequiresTwoFactor.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task EmailPasswordLogin_Should_login_user_with_correct_email_and_password()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await _back.SetPassword(user.Email, "My@nEw@strong@P4ssword");

        // Act
        var result = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert
        result.ShouldBeSuccess();
        result.Success.UserId.Should().BeGreaterThan(0);
        result.Success.InstitutionId.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task EmailPasswordLogin_Should_reset_failed_attempts_after_successful_login()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await _back.SetPassword(user.Email, "My@nEw@strong@P4ssword");

        var wrongPassword = "WrongPassword123!";

        // Act - 2 failed attempts (not enough to lockout)
        await client.EmailPasswordLogin(user.Email, wrongPassword);
        await client.EmailPasswordLogin(user.Email, wrongPassword);

        var successLogin = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // 2 more failed attempts after reset should not trigger lockout
        await client.EmailPasswordLogin(user.Email, wrongPassword);
        await client.EmailPasswordLogin(user.Email, wrongPassword);

        var attempt3 = await client.EmailPasswordLogin(user.Email, wrongPassword);

        // Assert
        successLogin.ShouldBeSuccess();
        attempt3.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    #endregion
}
