using Estud.Back.Features.Identity.ResetPassword;

namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(nameof(InvalidPasswords))]
    public async Task ResetPassword_Should_not_reset_password_to_a_weak_one(string password)
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(WeakPassword.I);
    }

    private static IEnumerable<object[]> InvalidPasswords()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "capi",
            "capi123",
            "Capi123",
            "lalal.com",
            "12@3lalala",
            "5816811681816",
        })
        {
            yield return [role];
        }
    }

    [Test]
    public async Task ResetPassword_Should_record_activity_when_reset_password_is_weak()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, "weak");

        // Assert
        await response.AssertBadRequest(WeakPassword.I);
    }

    #region Business Logic Errors

    [Test]
    public async Task ResetPassword_Should_not_reset_password_with_wrong_token()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = "token_errado_lalala";

        // Act
        var response = await client.ResetPassword(token, "My@nEw@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(InvalidResetPasswordToken.I);
    }

    [Test]
    public async Task ResetPassword_Should_not_reset_password_twice_with_same_token()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        await client.ResetPassword(token!, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.ResetPassword(token!, "My@nEw@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(InvalidResetPasswordToken.I);
    }

    [Test]
    public async Task ResetPassword_Should_not_reset_password_with_expired_token()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var tokenId = await _back.GetResetPasswordToken(user.Email);

        // Force token expiration via DbContext
        await using var ctx = _back.GetDbContext();
        var resetToken = await ctx.ResetPasswordTokens.FirstAsync(r => r.Id == Guid.Parse(tokenId!));
        resetToken.ExpiresAt = DateTime.UtcNow.AddMinutes(-1);
        await ctx.SaveChangesAsync();

        // Act
        var response = await client.ResetPassword(tokenId!, "My@nEw@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(InvalidResetPasswordToken.I);
    }

    [Test]
    public async Task ResetPassword_Should_not_reset_password_with_invalidated_token_after_security_stamp_change()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        // Request two reset tokens
        await client.SendResetPasswordToken(user.Email);
        var firstToken = await _back.GetResetPasswordToken(user.Email);

        await client.SendResetPasswordToken(user.Email);
        var secondToken = await _back.GetResetPasswordToken(user.Email);

        // Use second token (changes password + security stamp, invalidating the first)
        await client.ResetPassword(secondToken!, "My@nEw@strong@P4ssword");

        // Act - try to use first token (entity exists, not used, not expired, but Identity token is invalid)
        var response = await client.ResetPassword(firstToken!, "An0ther@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(InvalidResetPasswordToken.I);
    }

    #endregion

    #region Happy Path

    [Test]
    public async Task ResetPassword_Should_reset_password()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, "My@nEw@strong@P4ssword");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var ctx = _back.GetDbContext();
        var webUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        webUser!.EmailConfirmed.Should().BeTrue();
    }

    [Test]
    public async Task ResetPassword_Should_login_with_new_password_after_reset()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        await client.ResetPassword(token!, "My@nEw@strong@P4ssword");

        // Act
        var response = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");

        // Assert
        response.ShouldBeSuccess();
        response.Success.UserId.Should().Be(user.Id);

        await using var ctx = _back.GetDbContext();
        var webUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        webUser!.EmailConfirmed.Should().BeTrue();
    }

    [Test]
    public async Task ResetPassword_Should_use_latest_token_when_requesting_reset_multiple_times()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var user = await client.RegisterUser(DataGen.Email).Success();

        // Request reset twice
        await client.SendResetPasswordToken(user.Email);
        var firstToken = await _back.GetResetPasswordToken(user.Email);

        await client.SendResetPasswordToken(user.Email);
        var secondToken = await _back.GetResetPasswordToken(user.Email);

        // Tokens should be different
        firstToken.Should().NotBe(secondToken);

        // Act - use the latest token
        var response = await client.ResetPassword(secondToken!, "My@nEw@strong@P4ssword");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify login works
        var loginResponse = await client.EmailPasswordLogin(user.Email, "My@nEw@strong@P4ssword");
        loginResponse.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var webUser = await ctx.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        webUser!.EmailConfirmed.Should().BeTrue();
    }

    #endregion
}
