using Estud.Back.Auth.Schemes;
using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetRole = nameof(GetRole);
    public const string GetRoles = nameof(GetRoles);
    public const string CreateRole = nameof(CreateRole);
    public const string UpdateRole = nameof(UpdateRole);
    public const string GetDefaultRoles = nameof(GetDefaultRoles);
    public const string GetInstitutionRole = nameof(GetInstitutionRole);
    public const string GetInstitutionRoles = nameof(GetInstitutionRoles);
    public const string FilterInstitutionRoles = nameof(FilterInstitutionRoles);
    public const string GetAvailableInstitutionRoles = nameof(GetAvailableInstitutionRoles);

    public const string GetSsoConfiguration = nameof(GetSsoConfiguration);
    public const string GetSsoConfigurations = nameof(GetSsoConfigurations);
    public const string CreateSsoConfiguration = nameof(CreateSsoConfiguration);
    public const string UpdateSsoConfiguration = nameof(UpdateSsoConfiguration);

    public const string SetupTwoFactor = nameof(SetupTwoFactor);
    public const string GetTwoFactorKey = nameof(GetTwoFactorKey);
    public const string TwoFactorSetupLogin = nameof(TwoFactorSetupLogin);
    public const string GetTwoFactorEnforcement = nameof(GetTwoFactorEnforcement);
    public const string SetTwoFactorEnforcement = nameof(SetTwoFactorEnforcement);

    public const string Logout = nameof(Logout);
    public const string GetPermissions = nameof(GetPermissions);

    public static AuthorizationBuilder AddIdentityPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetRole, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetRoles, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(CreateRole, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(UpdateRole, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetPermissions, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetDefaultRoles, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetInstitutionRoles, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetInstitutionRole, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(FilterInstitutionRoles, UserType.Manager, EstudPermissions.ManageRoles)
            .AddEstudPolicy(GetAvailableInstitutionRoles, UserType.Manager, EstudPermissions.ManageRoles);

        builder
            .AddEstudPolicy(GetSsoConfiguration, UserType.Manager, EstudPermissions.ManageSso)
            .AddEstudPolicy(GetSsoConfigurations, UserType.Manager, EstudPermissions.ManageSso)
            .AddEstudPolicy(CreateSsoConfiguration, UserType.Manager, EstudPermissions.ManageSso)
            .AddEstudPolicy(UpdateSsoConfiguration, UserType.Manager, EstudPermissions.ManageSso);

        builder
            .AddEstudPolicy(GetTwoFactorEnforcement, UserType.Manager, EstudPermissions.ManageTwoFactor)
            .AddEstudPolicy(SetTwoFactorEnforcement, UserType.Manager, EstudPermissions.ManageTwoFactor);

        builder
            .AddEstudPolicy(Logout);

        // Os endpoints de setup do 2FA aceitam dois schemes: o Bearer full (setup
        // voluntário de quem já está logado) e o cookie temp TwoFactorSetup (fluxo
        // enforced, quando o usuário obrigado ainda não configurou o 2FA).
        builder
            .AddEstudPolicy(SetupTwoFactor, [JwtBearerScheme.Name, TwoFactorSetupScheme.Name])
            .AddEstudPolicy(GetTwoFactorKey, [JwtBearerScheme.Name, TwoFactorSetupScheme.Name]);

        // Login pós-setup enforced: só aceita a credencial de setup — troca o cookie
        // temp pelo JWT full depois que o 2FA foi habilitado.
        builder
            .AddEstudPolicy(TwoFactorSetupLogin, [TwoFactorSetupScheme.Name]);

        return builder;
    }
}
