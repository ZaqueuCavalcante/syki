using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetStudents = nameof(GetStudents);
    public const string CreateStudent = nameof(CreateStudent);

    public static AuthorizationBuilder AddStudentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetStudents, UserType.Manager, SykiPermissions.ManageStudents)
            .AddSykiPolicy(CreateStudent, UserType.Manager, SykiPermissions.ManageStudents);

        return builder;
    }
}
