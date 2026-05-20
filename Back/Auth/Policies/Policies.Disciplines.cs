using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetDisciplines = nameof(GetDisciplines);
    public const string CreateDiscipline = nameof(CreateDiscipline);

    public static AuthorizationBuilder AddDisciplinesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetDisciplines, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(CreateDiscipline, SykiPermissions.ManageDisciplines);

        return builder;
    }
}
