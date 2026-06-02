using System.Net.Http.Json;
using Syki.Back.Features.Identity.GetRoles;
using Syki.Back.Features.Identity.CreateRole;
using Syki.Back.Features.Identity.ResetPassword;
using Syki.Back.Features.Identity.MagicLinkLogin;
using Syki.Back.Features.Identity.SetupTwoFactor;
using Syki.Back.Features.Identity.TwoFactorLogin;
using Syki.Back.Features.Identity.GetTwoFactorKey;
using Syki.Back.Features.Identity.EmailPasswordLogin;
using Syki.Back.Features.Identity.SendResetPasswordToken;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<MagicLinkLoginOut, ErrorOut>> MagicLinkLogin(string token)
    {
        var data = new MagicLinkLoginIn { Token = token };

        var response = await http.PostAsJsonAsync("identity/magic-link-login", data);

        return await response.Resolve<MagicLinkLoginOut>();
    }

    public async Task<OneOf<EmailPasswordLoginOut, ErrorOut>> EmailPasswordLogin(string email, string password)
    {
        var data = new EmailPasswordLoginIn { Email = email, Password = password };

        var response = await http.PostAsJsonAsync("identity/email-password-login", data);

        return await response.Resolve<EmailPasswordLoginOut>();
    }

    public async Task<HttpResponseMessage> Logout()
    {
        return await http.PostAsJsonAsync("identity/logout", new {});
    }

    public async Task<OneOf<GetTwoFactorKeyOut, ErrorOut>> GetTwoFactorKey()
    {
        var response = await http.GetAsync("identity/2fa-key");
        return await response.Resolve<GetTwoFactorKeyOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> SetupTwoFactor(string token)
    {
        var data = new SetupTwoFactorIn { Token = token };
        var response = await http.PostAsJsonAsync("identity/2fa-setup", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<TwoFactorLoginOut, ErrorOut>> TwoFactorLogin(string? token)
    {
        var body = new TwoFactorLoginIn { Token = token };

        var response = await http.PostAsJsonAsync("identity/2fa-login", body);

        return await response.Resolve<TwoFactorLoginOut>();
    }

    public async Task<HttpResponseMessage> SendResetPasswordToken(string email)
    {
        var data = new SendResetPasswordTokenIn { Email = email };
        return await http.PostAsJsonAsync("identity/reset-password-token", data);
    }

    public async Task<HttpResponseMessage> ResetPassword(string token, string password)
    {
        var data = new ResetPasswordIn { Token = token, Password = password };
        return await http.PostAsJsonAsync("identity/reset-password", data);
    }

    public async Task<OneOf<CreateRoleOut, ErrorOut>> CreateRole(
        string name = "Admin",
        string description = "Administrador com acesso total",
        UserType baseType = UserType.Manager,
        List<int>? permissions = null
    ) {
        var data = new CreateRoleIn { Name = name, Description = description, BaseType = baseType, Permissions = permissions ?? [] };
        var response = await http.PostAsJsonAsync("identity/roles", data);
        return await response.Resolve<CreateRoleOut>();
    }

    public async Task<OneOf<GetRolesOut, ErrorOut>> GetRoles()
    {
        var response = await http.GetAsync("identity/roles");
        return await response.Resolve<GetRolesOut>();
    }
}
