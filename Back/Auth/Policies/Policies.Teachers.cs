using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateTeacher = nameof(CreateTeacher);

    public static AuthorizationBuilder AddTeachersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(CreateTeacher, SykiPermissions.ManageTeachers);

        return builder;
    }
}
