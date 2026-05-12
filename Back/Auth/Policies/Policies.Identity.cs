using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateRole = nameof(CreateRole);
    public const string UpdateRole = nameof(UpdateRole);
    public const string GetDefaultRoles = nameof(GetDefaultRoles);
    public const string GetInstitutionRole = nameof(GetInstitutionRole);
    public const string GetInstitutionRoles = nameof(GetInstitutionRoles);
    public const string FilterInstitutionRoles = nameof(FilterInstitutionRoles);
    public const string GetAvailableInstitutionRoles = nameof(GetAvailableInstitutionRoles);

    public const string CreateSsoConfiguration = nameof(CreateSsoConfiguration);
    public const string UpdateSsoConfiguration = nameof(UpdateSsoConfiguration);
    public const string GetSsoConfigurations = nameof(GetSsoConfigurations);

    public const string SetupTwoFactor = nameof(SetupTwoFactor);
    public const string GetTwoFactorKey = nameof(GetTwoFactorKey);

    public const string Logout = nameof(Logout);
    public const string GetPermissions = nameof(GetPermissions);

    public static AuthorizationBuilder AddIdentityPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(CreateRole, SykiPermissions.ManageRoles)
            .AddSykiPolicy(UpdateRole, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetDefaultRoles, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetInstitutionRoles, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetInstitutionRole, SykiPermissions.ManageRoles)
            .AddSykiPolicy(FilterInstitutionRoles, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetAvailableInstitutionRoles, SykiPermissions.ManageRoles);

        builder
            .AddSykiPolicy(CreateSsoConfiguration, SykiPermissions.ManageSso)
            .AddSykiPolicy(UpdateSsoConfiguration, SykiPermissions.ManageSso)
            .AddSykiPolicy(GetSsoConfigurations, SykiPermissions.ManageSso);

        builder
            .AddSykiPolicy(SetupTwoFactor)
            .AddSykiPolicy(GetTwoFactorKey);

        builder
            .AddSykiPolicy(Logout)
            .AddSykiPolicy(GetPermissions);

        return builder;
    }
}
