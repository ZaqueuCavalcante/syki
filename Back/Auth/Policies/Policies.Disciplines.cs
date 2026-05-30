using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetDiscipline = nameof(GetDiscipline);
    public const string GetDisciplines = nameof(GetDisciplines);
    public const string CreateDiscipline = nameof(CreateDiscipline);
    public const string UpdateDiscipline = nameof(UpdateDiscipline);

    public const string AddDisciplineCourses = nameof(AddDisciplineCourses);
    public const string GetDisciplinePotentialCourses = nameof(GetDisciplinePotentialCourses);

    public static AuthorizationBuilder AddDisciplinesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetDiscipline, UserType.Manager, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(GetDisciplines, UserType.Manager, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(CreateDiscipline, UserType.Manager, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(UpdateDiscipline, UserType.Manager, SykiPermissions.ManageDisciplines);

        builder
            .AddSykiPolicy(AddDisciplineCourses, UserType.Manager, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(GetDisciplinePotentialCourses, UserType.Manager, SykiPermissions.ManageDisciplines);

        return builder;
    }
}
