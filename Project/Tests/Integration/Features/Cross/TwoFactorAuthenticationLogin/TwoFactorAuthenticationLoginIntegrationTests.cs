namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_login_when_supply_right_totp()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();
        var user = client.User;

        var keyResponse = await client.Cross.GetTwoFactorAuthenticationKey();
        var token = keyResponse.Key.GenerateTOTP();
        await client.Cross.SetupTwoFactorAuthentication(token);

        await client.Cross.Logout();
        await client.Cross.Login(user.Email, ExatoCSPassword);

        // Act
        var response = await client.Cross.TwoFactorAuthenticationLogin(token);

        // Assert
        var loginOut = response.Success;
        loginOut.Id.Should().Be(user.Id);
        loginOut.Name.Should().Be(user.Name);
        loginOut.Email.Should().Be(user.Email);
        loginOut.Features.Should().NotBeEmpty();
        loginOut.Role.Should().Be("OfficeCustomerSuccess");
    }

    [Test]
    public async Task Should_not_login_with_right_totp_but_without_supply_email_and_password()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        var keyResponse = await client.Cross.GetTwoFactorAuthenticationKey();
        var totp = keyResponse.Key.GenerateTOTP();
        await client.Cross.SetupTwoFactorAuthentication(totp);

        // Act
        var response = await client.Cross.TwoFactorAuthenticationLogin(totp);

        // Assert
        response.ShouldBeError(Invalid2faToken.I);
    }

    [Test]
    public async Task Should_not_login_with_right_totp_but_without_setup_two_factor()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        var keyResponse = await client.Cross.GetTwoFactorAuthenticationKey();
        var totp = keyResponse.Key.GenerateTOTP();

        // Act
        var response = await client.Cross.TwoFactorAuthenticationLogin(totp);

        // Assert
        response.ShouldBeError(Invalid2faToken.I);
    }

    [Test]
    public async Task Should_not_login_when_supply_wrong_totp()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();
        var user = client.User;

        await client.Cross.GetTwoFactorAuthenticationKey();

        await client.Cross.Logout();
        await client.Cross.Login(user.Email, ExatoCSPassword);
        var randomToken = Guid.NewGuid().ToHashCode().ToString()[..6];

        // Act
        var response = await client.Cross.TwoFactorAuthenticationLogin(randomToken);

        // Assert
        response.ShouldBeError(Invalid2faToken.I);
    }
}
