using System.Net.Http.Json;
using Syki.Back.Features.Identity.GetRole;
using Syki.Back.Features.Identity.GetRoles;
using Syki.Back.Features.Identity.CreateRole;
using Syki.Back.Features.Identity.UpdateRole;
using Syki.Back.Features.Identity.GetPermissions;
using Syki.Back.Features.Identity.ResetPassword;
using Syki.Back.Features.Identity.MagicLinkLogin;
using Syki.Back.Features.Identity.SetupTwoFactor;
using Syki.Back.Features.Identity.TwoFactorLogin;
using Syki.Back.Features.Identity.GetTwoFactorKey;
using Syki.Back.Features.Identity.EmailPasswordLogin;
using Syki.Back.Features.Identity.GetSsoConfiguration;
using Syki.Back.Features.Identity.CheckSsoAvailability;
using Syki.Back.Features.Identity.CheckSocialLoginAvailability;
using Syki.Back.Features.Identity.CreateSsoConfiguration;
using Syki.Back.Features.Identity.UpdateSsoConfiguration;
using Syki.Back.Features.Identity.SendResetPasswordToken;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<MagicLinkLoginOut, ErrorOut>> MagicLinkLogin(string? token)
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

    public async Task<OneOf<GetRoleOut, ErrorOut>> GetRole(int id)
    {
        var response = await http.GetAsync($"identity/roles/{id}");
        return await response.Resolve<GetRoleOut>();
    }

    public async Task<OneOf<UpdateRoleOut, ErrorOut>> UpdateRole(
        int id,
        string name = "Admin",
        string description = "Administrador com acesso total",
        UserType baseType = UserType.Manager,
        List<int>? permissions = null
    ) {
        var data = new UpdateRoleIn { Id = id, Name = name, Description = description, BaseType = baseType, Permissions = permissions ?? [] };
        var response = await http.PutAsJsonAsync("identity/roles", data);
        return await response.Resolve<UpdateRoleOut>();
    }

    public async Task<OneOf<GetPermissionsOut, ErrorOut>> GetPermissions()
    {
        var response = await http.GetAsync("identity/permissions");
        return await response.Resolve<GetPermissionsOut>();
    }

    public async Task<OneOf<CreateSsoConfigurationOut, ErrorOut>> CreateSsoConfiguration(
        SsoProviderType providerType = SsoProviderType.AzureAd,
        string authority = "https://login.microsoftonline.com/tenant-id/v2.0",
        string clientId = "00000000-0000-0000-0000-000000000000",
        string clientSecret = "client-secret-value"
    ) {
        var data = new CreateSsoConfigurationIn
        {
            ProviderType = providerType,
            Authority = authority,
            ClientId = clientId,
            ClientSecret = clientSecret,
        };
        var response = await http.PostAsJsonAsync("identity/sso/configurations", data);
        return await response.Resolve<CreateSsoConfigurationOut>();
    }

    public async Task<OneOf<CheckSocialLoginAvailabilityOut, ErrorOut>> CheckSocialLoginAvailability()
    {
        var response = await http.GetAsync("identity/social-login/check-availability");
        return await response.Resolve<CheckSocialLoginAvailabilityOut>();
    }

    public async Task<OneOf<CheckSsoAvailabilityOut, ErrorOut>> CheckSsoAvailability(string email = "usuario@empresa.com.br")
    {
        var data = new CheckSsoAvailabilityIn { Email = email };
        var response = await http.PostAsJsonAsync("identity/sso/check-availability", data);
        return await response.Resolve<CheckSsoAvailabilityOut>();
    }

    public async Task<OneOf<GetSsoConfigurationOut, ErrorOut>> GetSsoConfiguration()
    {
        var response = await http.GetAsync("identity/sso/configuration");
        return await response.Resolve<GetSsoConfigurationOut>();
    }

    public async Task<OneOf<UpdateSsoConfigurationOut, ErrorOut>> UpdateSsoConfiguration(
        Guid id,
        SsoProviderType providerType = SsoProviderType.AzureAd,
        string authority = "https://login.microsoftonline.com/tenant-id/v2.0",
        string clientId = "00000000-0000-0000-0000-000000000000",
        string? clientSecret = null,
        bool isActive = true,
        bool requireSso = false
    ) {
        var data = new UpdateSsoConfigurationIn
        {
            ProviderType = providerType,
            Authority = authority,
            ClientId = clientId,
            ClientSecret = clientSecret,
            IsActive = isActive,
            RequireSso = requireSso,
        };
        var response = await http.PutAsJsonAsync($"identity/sso/configurations/{id}", data);
        return await response.Resolve<UpdateSsoConfigurationOut>();
    }
}
