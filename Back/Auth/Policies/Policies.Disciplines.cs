using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetDiscipline = nameof(GetDiscipline);
    public const string GetDisciplines = nameof(GetDisciplines);
    public const string CreateDiscipline = nameof(CreateDiscipline);
    public const string UpdateDiscipline = nameof(UpdateDiscipline);

    public const string AddDisciplineCourses = nameof(AddDisciplineCourses);
    public const string RemoveDisciplineCourse = nameof(RemoveDisciplineCourse);
    public const string GetDisciplinePotentialCourses = nameof(GetDisciplinePotentialCourses);

    public static AuthorizationBuilder AddDisciplinesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetDiscipline, UserType.Manager, EstudPermissions.ManageDisciplines)
            .AddEstudPolicy(GetDisciplines, UserType.Manager, EstudPermissions.ManageDisciplines)
            .AddEstudPolicy(CreateDiscipline, UserType.Manager, EstudPermissions.ManageDisciplines)
            .AddEstudPolicy(UpdateDiscipline, UserType.Manager, EstudPermissions.ManageDisciplines);

        builder
            .AddEstudPolicy(AddDisciplineCourses, UserType.Manager, EstudPermissions.ManageDisciplines)
            .AddEstudPolicy(RemoveDisciplineCourse, UserType.Manager, EstudPermissions.ManageDisciplines)
            .AddEstudPolicy(GetDisciplinePotentialCourses, UserType.Manager, EstudPermissions.ManageDisciplines);

        return builder;
    }
}
