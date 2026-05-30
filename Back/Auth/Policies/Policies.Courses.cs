using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourses = nameof(GetCourses);
    public const string CreateCourse = nameof(CreateCourse);
    public const string UpdateCourse = nameof(UpdateCourse);

    public const string GetCourseDisciplines = nameof(GetCourseDisciplines);

    public static AuthorizationBuilder AddCoursesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCourses, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(CreateCourse, UserType.Manager, SykiPermissions.ManageCourses)
            .AddSykiPolicy(UpdateCourse, UserType.Manager, SykiPermissions.ManageCourses);

        builder
            .AddSykiPolicy(GetCourseDisciplines, UserType.Manager, SykiPermissions.ManageCourses);

        return builder;
    }
}
