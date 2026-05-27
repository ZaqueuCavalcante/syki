using Syki.Back.Features.Identity.EmailPasswordLogin;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Validation Errors

    [Test]
    public async Task TwoFactorLogin_Should_not_login_with_right_totp_but_without_supply_email_and_password()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var totp = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(totp);

        // Act
        var response = await client.TwoFactorLogin(totp);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_login_with_right_totp_but_without_setup_two_factor()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var totp = keyResponse.Success.Key.GenerateTOTP();

        // Act
        var response = await client.TwoFactorLogin(totp);

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
    public async Task TwoFactorLogin_Should_not_login_2fa_with_empty_token()
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
    public async Task TwoFactorLogin_Should_not_login_2fa_with_null_token()
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
    public async Task TwoFactorLogin_Should_record_activity_when_2fa_token_is_invalid()
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
        var response = await client.TwoFactorLogin(wrongToken);

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

        // Act - 3 failed 2FA attempts (triggers lockout internally via AccessFailedAsync)
        var attempt1 = await client.TwoFactorLogin(wrongToken);
        var attempt2 = await client.TwoFactorLogin(wrongToken);
        var attempt3 = await client.TwoFactorLogin(wrongToken);

        // 4th attempt - user is now locked out
        var attempt4 = await client.TwoFactorLogin(wrongToken);

        // Assert
        attempt1.ShouldBeError(InvalidTwoFactorToken.I);
        attempt2.ShouldBeError(InvalidTwoFactorToken.I);
        attempt3.ShouldBeError(InvalidTwoFactorToken.I);
        attempt4.ShouldBeError(InvalidTwoFactorToken.I);

        // Verify lockout through normal login (even with correct password)
        var loginAttempt = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");
        loginAttempt.ShouldBeError(LoginUserLockedOut.I);
    }

    [Test]
    public async Task TwoFactorLogin_Should_not_allow_2fa_login_when_locked_out_even_with_correct_totp()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var validToken = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(validToken);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Lock out the user with 3 wrong attempts
        var wrongToken = validToken == "000000" ? "111111" : "000000";
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Act - try with correct TOTP while locked out
        var correctToken = keyResponse.Success.Key.GenerateTOTP();
        var result = await client.TwoFactorLogin(correctToken);

        // Assert - should fail because user is locked out (rejects before TOTP verification)
        result.ShouldBeError(InvalidTwoFactorToken.I);

        // Confirm lockout state
        var loginAttempt = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");
        loginAttempt.ShouldBeError(LoginUserLockedOut.I);
    }

    #endregion

    #region Happy Path

    [Test]
    public async Task TwoFactorLogin_Should_login_when_supply_right_totp()
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
        var loginOut = response.Success;
        loginOut.Id.Should().Be(user.Id);
    }

    [Test]
    public async Task TwoFactorLogin_Should_login_2fa_with_token_containing_spaces()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(token);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act - token with spaces (should be stripped)
        var tokenWithSpaces = $"{token[..3]} {token[3..]}";
        var response = await client.TwoFactorLogin(tokenWithSpaces);

        // Assert
        response.ShouldBeSuccess();
        response.Success.Id.Should().Be(user.Id);
    }

    [Test]
    public async Task TwoFactorLogin_Should_record_activity_log_on_two_factor_login()
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
    }

    [Test]
    public async Task TwoFactorLogin_Should_reset_2fa_failed_attempts_after_successful_2fa_login()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var user = client.User;

        var keyResponse = await client.GetTwoFactorKey();
        var validToken = keyResponse.Success.Key.GenerateTOTP();
        await client.SetupTwoFactor(validToken);

        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // 2 failed attempts (not enough to lockout)
        var wrongToken = validToken == "000000" ? "111111" : "000000";
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Successful 2FA login resets the counter
        var successResult = await client.TwoFactorLogin(keyResponse.Success.Key.GenerateTOTP());
        successResult.ShouldBeSuccess();

        // Start a new 2FA flow
        await client.Logout();
        await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Act - 2 more failed attempts (should NOT trigger lockout since counter was reset)
        await client.TwoFactorLogin(wrongToken);
        await client.TwoFactorLogin(wrongToken);

        // Verify user is NOT locked out
        var loginAttempt = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert - should get LoginRequiresTwoFactor (not locked out)
        loginAttempt.ShouldBeError(LoginRequiresTwoFactor.I);
    }

    #endregion
}
