using Estud.Back.Features.Identity.EmailPasswordLogin;
using Estud.Back.Features.Identity.TwoFactorSetupLogin;

namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_TwoFactorSetupLogin_Should_not_login_without_setup_credential()
    {
        // Arrange - sem cookie de setup
        var client = _back.GetTestsClient();

        // Act
        var result = await client.TwoFactorSetupLogin();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Identity_TwoFactorSetupLogin_Should_not_accept_bearer_credential()
    {
        // Arrange - usuário logado com JWT full (não com o cookie de setup)
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.TwoFactorSetupLogin();

        // Assert - a policy só aceita o TwoFactorSetupScheme
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Identity_TwoFactorSetupLogin_Should_not_login_when_2fa_not_yet_enabled()
    {
        // Arrange - login enforced deixa o cookie de setup, mas o 2FA ainda não foi habilitado
        var client = await _back.LoggedAsDirector();
        var roles = await client.GetRoles().Success();
        var role = roles.Items.First(r => r.BaseType == UserType.Manager);
        await client.SetTwoFactorEnforcement(role.Id, true);

        await client.Logout();
        var login = await client.EmailPasswordLogin(client.User.Email, "My@nEw@strong@P4ssword");
        login.ShouldBeError(LoginTwoFactorEnforced.I);

        // Act - tenta finalizar o login sem ter concluído o setup
        var result = await client.TwoFactorSetupLogin();

        // Assert
        result.ShouldBeError(TwoFactorSetupNotCompleted.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_TwoFactorSetupLogin_Should_login_with_full_jwt_after_enforced_setup()
    {
        // Arrange - role obrigada, usuário sem 2FA, logado apenas pelo cookie de setup
        var client = await _back.LoggedAsDirector();
        var roles = await client.GetRoles().Success();
        var role = roles.Items.First(r => r.BaseType == UserType.Manager);
        await client.SetTwoFactorEnforcement(role.Id, true);

        await client.Logout();
        var login = await client.EmailPasswordLogin(client.User.Email, "My@nEw@strong@P4ssword");
        login.ShouldBeError(LoginTwoFactorEnforced.I);

        // Antes de finalizar: endpoint protegido por Bearer é rejeitado
        var beforeLogin = await client.GetRoles();
        beforeLogin.ShouldBeError(HttpStatusCode.Unauthorized);

        // Conclui o setup usando o cookie de setup
        var keyResponse = await client.GetTwoFactorKey().Success();
        var token = keyResponse.Key.GenerateTOTP();
        await client.SetupTwoFactor(token).Success();

        // Act - troca o cookie de setup pelo JWT full
        var result = await client.TwoFactorSetupLogin().Success();

        // Assert - logado de verdade e acessa endpoints por Bearer
        result.Id.Should().BeGreaterThan(0);
        result.InstitutionId.Should().BeGreaterThan(0);

        var afterLogin = await client.GetRoles();
        afterLogin.ShouldBeSuccess();
    }

    [Test]
    public async Task Identity_TwoFactorSetupLogin_Should_invalidate_setup_cookie_after_login()
    {
        // Arrange - role obrigada, usuário sem 2FA, logado apenas pelo cookie de setup
        var client = await _back.LoggedAsDirector();
        var roles = await client.GetRoles().Success();
        var role = roles.Items.First(r => r.BaseType == UserType.Manager);
        await client.SetTwoFactorEnforcement(role.Id, true);

        await client.Logout();
        var login = await client.EmailPasswordLogin(client.User.Email, "My@nEw@strong@P4ssword");
        login.ShouldBeError(LoginTwoFactorEnforced.I);

        var keyResponse = await client.GetTwoFactorKey().Success();
        var token = keyResponse.Key.GenerateTOTP();
        await client.SetupTwoFactor(token).Success();

        // Primeiro login troca o cookie de setup pelo JWT full
        await client.TwoFactorSetupLogin().Success();

        // Act - segunda chamada: o cookie de setup já foi invalidado (SignOut), só resta o Bearer
        var secondLogin = await client.TwoFactorSetupLogin();

        // Assert - a policy só aceita o TwoFactorSetupScheme, que não existe mais → 401
        secondLogin.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion
}
