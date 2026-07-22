using System.Net.Http.Json;
using Estud.Back.Features.Identity.GetRole;
using Estud.Back.Features.Identity.GetRoles;
using Estud.Back.Features.Identity.CreateRole;
using Estud.Back.Features.Identity.UpdateRole;
using Estud.Back.Features.Identity.ResetPassword;
using Estud.Back.Features.Identity.GetPermissions;
using Estud.Back.Features.Identity.MagicLinkLogin;
using Estud.Back.Features.Identity.SetupTwoFactor;
using Estud.Back.Features.Identity.TwoFactorLogin;
using Estud.Back.Features.Identity.GetTwoFactorKey;
using Estud.Back.Features.Identity.EmailPasswordLogin;
using Estud.Back.Features.Identity.GetSsoConfiguration;
using Estud.Back.Features.Identity.TwoFactorSetupLogin;
using Estud.Back.Features.Identity.CheckSsoAvailability;
using Estud.Back.Features.Identity.CreateSsoConfiguration;
using Estud.Back.Features.Identity.UpdateSsoConfiguration;
using Estud.Back.Features.Identity.SendResetPasswordToken;
using Estud.Back.Features.Identity.SetTwoFactorEnforcement;
using Estud.Back.Features.Identity.GetTwoFactorEnforcement;
using Estud.Back.Features.Identity.CheckSocialLoginAvailability;

namespace Estud.Tests.Integration.Clients;

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

    public async Task<OneOf<TwoFactorSetupLoginOut, ErrorOut>> TwoFactorSetupLogin()
    {
        var response = await http.PostAsJsonAsync("identity/2fa-setup-login", new {});

        return await response.Resolve<TwoFactorSetupLoginOut>();
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
        List<int>? permissions = null
    ) {
        var data = new UpdateRoleIn { Id = id, Name = name, Description = description, Permissions = permissions ?? [] };
        var response = await http.PutAsJsonAsync("identity/roles", data);
        return await response.Resolve<UpdateRoleOut>();
    }

    public async Task<OneOf<GetPermissionsOut, ErrorOut>> GetPermissions()
    {
        var response = await http.GetAsync("identity/permissions");
        return await response.Resolve<GetPermissionsOut>();
    }

    public async Task<OneOf<GetTwoFactorEnforcementOut, ErrorOut>> GetTwoFactorEnforcement()
    {
        var response = await http.GetAsync("identity/2fa-enforcement");
        return await response.Resolve<GetTwoFactorEnforcementOut>();
    }

    public async Task<OneOf<SetTwoFactorEnforcementOut, ErrorOut>> SetTwoFactorEnforcement(int roleId, bool required)
    {
        var data = new SetTwoFactorEnforcementIn { RoleId = roleId, Required = required };
        var response = await http.PutAsJsonAsync("identity/2fa-enforcement", data);
        return await response.Resolve<SetTwoFactorEnforcementOut>();
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
