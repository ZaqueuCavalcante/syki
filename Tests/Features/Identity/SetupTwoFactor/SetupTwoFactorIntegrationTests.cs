using Estud.Back.Features.Identity.EmailPasswordLogin;
using Estud.Back.Features.Identity.TwoFactorSetupLogin;

namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_SetupTwoFactor_Should_not_setup_2fa_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.SetupTwoFactor("000000");

        // Assert
        response.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCaseSource(nameof(Invalid2faTokens))]
    public async Task SetupTwoFactor_Should_not_setup_2fa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.SetupTwoFactor(token);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
    }

    private static IEnumerable<object[]> Invalid2faTokens()
    {
        foreach (var token in new List<string>()
        {
            "",
            " ",
            "5464",
            "estud",
            "123456",
            "lalal.com",
            "5816811681816",
        })
        {
            yield return [token];
        }
    }

    [Test]
    public async Task SetupTwoFactor_Should_not_setup_2fa_with_wrong_token_under_setup_cookie()
    {
        // Arrange - login enforced deixa apenas o cookie de setup (usuário obrigado, sem 2FA)
        var client = await _back.LoggedAsDirector();
        var roles = await client.GetRoles().Success();
        var role = roles.Items.First(r => r.BaseType == UserType.Manager);
        await client.SetTwoFactorEnforcement(role.Id, true);

        await client.Logout();
        var login = await client.EmailPasswordLogin(client.User.Email, "My@nEw@strong@P4ssword");
        login.ShouldBeError(LoginTwoFactorEnforced.I);

        // Act - conclui o setup com um token inválido usando o cookie de setup
        var response = await client.SetupTwoFactor("123456");

        // Assert - o setup falha e o 2FA não é habilitado
        response.ShouldBeError(InvalidTwoFactorToken.I);

        // O usuário permanece no estado de setup: o cookie ainda não vira JWT full
        var setupLogin = await client.TwoFactorSetupLogin();
        setupLogin.ShouldBeError(TwoFactorSetupNotCompleted.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task SetupTwoFactor_Should_setup_2fa()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey().Success();
        var token = keyResponse.Key.GenerateTOTP();

        // Act
        var response = await client.SetupTwoFactor(token);

        // Assert
        response.ShouldBeSuccess();
        var key = await client.GetTwoFactorKey().Success();
        key.TwoFactorEnabled.Should().BeTrue();
    }

    #endregion
}
