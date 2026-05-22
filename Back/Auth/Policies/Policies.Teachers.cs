using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetTeachers = nameof(GetTeachers);
    public const string CreateTeacher = nameof(CreateTeacher);

    public static AuthorizationBuilder AddTeachersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetTeachers, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(CreateTeacher, SykiPermissions.ManageTeachers);

        return builder;
    }
}
