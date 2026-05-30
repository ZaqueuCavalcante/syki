using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetRoles = nameof(GetRoles);
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
            .AddSykiPolicy(GetRoles, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(CreateRole, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(UpdateRole, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetDefaultRoles, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetInstitutionRoles, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetInstitutionRole, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(FilterInstitutionRoles, UserType.Manager, SykiPermissions.ManageRoles)
            .AddSykiPolicy(GetAvailableInstitutionRoles, UserType.Manager, SykiPermissions.ManageRoles);

        builder
            .AddSykiPolicy(CreateSsoConfiguration, UserType.Manager, SykiPermissions.ManageSso)
            .AddSykiPolicy(UpdateSsoConfiguration, UserType.Manager, SykiPermissions.ManageSso)
            .AddSykiPolicy(GetSsoConfigurations, UserType.Manager, SykiPermissions.ManageSso);

        builder
            .AddSykiPolicy(SetupTwoFactor)
            .AddSykiPolicy(GetTwoFactorKey);

        builder
            .AddSykiPolicy(Logout)
            .AddSykiPolicy(GetPermissions);

        return builder;
    }
}
