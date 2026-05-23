using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourses = nameof(GetCourses);
    public const string CreateCourse = nameof(CreateCourse);
    public const string UpdateCourse = nameof(UpdateCourse);

    public static AuthorizationBuilder AddCoursesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCourses, SykiPermissions.ManageCourses)
            .AddSykiPolicy(CreateCourse, SykiPermissions.ManageCourses)
            .AddSykiPolicy(UpdateCourse, SykiPermissions.ManageCourses);

        return builder;
    }
}
