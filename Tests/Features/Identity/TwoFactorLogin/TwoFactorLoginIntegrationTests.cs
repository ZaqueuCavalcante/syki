using Estud.Back.Features.Identity.EmailPasswordLogin;

namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Validation errors

    [Test]
    public async Task TwoFactorLogin_Should_not_login_without_going_through_email_password_first()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var totp = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(totp);

        // Act - tries 2FA login without email+password initiation
        var response = await client.TwoFactorLogin(totp);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_when_2fa_is_not_setup()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        await client.GetTwoFactorKey();
        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.TwoFactorLogin(Random.Shared.Next(100000, 999999).ToString());

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_when_supply_wrong_totp()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        await client.GetTwoFactorKey();

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        var randomToken = Random.Shared.Next(100000, 999999).ToString();

        // Act
        var response = await client.TwoFactorLogin(randomToken);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_with_empty_token()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.TwoFactorLogin("");

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_with_null_token()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.TwoFactorLogin(null!);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_lockout_user_after_3_failed_2fa_attempts()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var validToken = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(validToken);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        var wrongToken = validToken == "000000" ? "111111" : "000000";

        // Act
        var attempt1 = await client.TwoFactorLogin(wrongToken);
        var attempt2 = await client.TwoFactorLogin(wrongToken);
        var attempt3 = await client.TwoFactorLogin(wrongToken);
        var attempt4 = await client.TwoFactorLogin(wrongToken);

        var loginAfterLockout = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert
        attempt1.ShouldBeError(InvalidTwoFactorToken.I);
        attempt2.ShouldBeError(InvalidTwoFactorToken.I);
        attempt3.ShouldBeError(InvalidTwoFactorToken.I);
        attempt4.ShouldBeError(InvalidTwoFactorToken.I);
        loginAfterLockout.ShouldBeError(LoginUserLockedOut.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_when_user_is_locked_out()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var validToken = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(validToken);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        var wrongToken = validToken == "000000" ? "111111" : "000000";
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Act - attempt with correct TOTP while locked out
        var correctToken = keyResponse.Success.Key.GenerateTOTP();
        var result = await client.TwoFactorLogin(correctToken);

        // Assert
        result.ShouldBeError(InvalidTwoFactorToken.I);
        var loginAttempt = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");
        loginAttempt.ShouldBeError(LoginUserLockedOut.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task TwoFactorLogin_Should_login_with_correct_totp()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.TwoFactorLogin(token);

        // Assert
        response.ShouldBeSuccess();
        response.Success.Id.Should().Be(user.Id);
    }

    [Test]
    public async Task TwoFactorLogin_Should_login_with_token_containing_spaces()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act - token with spaces should be stripped
        var tokenWithSpaces = $"{token[..3]} {token[3..]}";
        var response = await client.TwoFactorLogin(tokenWithSpaces);

        // Assert
        response.ShouldBeSuccess();
        response.Success.Id.Should().Be(user.Id);
    }

    [Test]
    public async Task TwoFactorLogin_Should_reset_failed_attempts_after_successful_login()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var validToken = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(validToken);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        var wrongToken = validToken == "000000" ? "111111" : "000000";
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Successful login resets failed attempt counter
        var successResult = await client.TwoFactorLogin(keyResponse.Success.Key.GenerateTOTP());
        successResult.ShouldBeSuccess();

        // Start new 2FA flow
        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // 2 more failed attempts should not trigger lockout (counter was reset)
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Act
        var loginAttempt = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert - LoginRequiresTwoFactor, not locked out
        loginAttempt.ShouldBeError(LoginRequiresTwoFactor.I);
    }

    #endregion
}
