using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCampi = nameof(GetCampi);
    public const string GetCampus = nameof(GetCampus);
    public const string CreateCampus = nameof(CreateCampus);
    public const string UpdateCampus = nameof(UpdateCampus);

    public static AuthorizationBuilder AddCampiPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCampi, UserType.Manager, SykiPermissions.ManageCampi)
            .AddSykiPolicy(GetCampus, UserType.Manager, SykiPermissions.ManageCampi)
            .AddSykiPolicy(CreateCampus, UserType.Manager, SykiPermissions.ManageCampi)
            .AddSykiPolicy(UpdateCampus, UserType.Manager, SykiPermissions.ManageCampi);

        return builder;
    }
}
