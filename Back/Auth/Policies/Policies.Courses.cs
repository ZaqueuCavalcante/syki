using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

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
            .AddSykiPolicy(GetCourse, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(GetCourses, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(CreateCourse, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(UpdateCourse, UserType.Manager, SykiPermissions.ManageCourses);

        builder
            .AddSykiPolicy(GetCourseDisciplines, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(AddCourseDisciplines, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(RemoveCourseDiscipline, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(GetCoursePotentialDisciplines, UserType.Manager, SykiPermissions.ManageCourses);

        return builder;
    }
}
