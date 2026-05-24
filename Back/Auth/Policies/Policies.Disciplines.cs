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
            .AddSykiPolicy(GetDiscipline, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(GetDisciplines, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(CreateDiscipline, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(UpdateDiscipline, SykiPermissions.ManageDisciplines);

        builder
            .AddSykiPolicy(AddDisciplineCourses, SykiPermissions.ManageDisciplines)
            .AddSykiPolicy(GetDisciplinePotentialCourses, SykiPermissions.ManageDisciplines);

        return builder;
    }
}
