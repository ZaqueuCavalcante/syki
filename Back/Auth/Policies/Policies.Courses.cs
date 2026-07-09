using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourse = nameof(GetCourse);
    public const string GetCourses = nameof(GetCourses);
    public const string CreateCourse = nameof(CreateCourse);
    public const string UpdateCourse = nameof(UpdateCourse);

    public const string GetCourseDisciplines = nameof(GetCourseDisciplines);
    public const string AddCourseDisciplines = nameof(AddCourseDisciplines);
    public const string RemoveCourseDiscipline = nameof(RemoveCourseDiscipline);
    public const string GetCoursePotentialDisciplines = nameof(GetCoursePotentialDisciplines);

    public static AuthorizationBuilder AddCoursesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetCourse, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(GetCourses, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(CreateCourse, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(UpdateCourse, UserType.Manager, EstudPermissions.ManageCourses);

        builder
            .AddEstudPolicy(GetCourseDisciplines, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(AddCourseDisciplines, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(RemoveCourseDiscipline, UserType.Manager, EstudPermissions.ManageCourses)
            .AddEstudPolicy(GetCoursePotentialDisciplines, UserType.Manager, EstudPermissions.ManageCourses);

        return builder;
    }
}
