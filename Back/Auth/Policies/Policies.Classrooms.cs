using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClassrooms = nameof(GetClassrooms);
    public const string CreateClassroom = nameof(CreateClassroom);

    public static AuthorizationBuilder AddClassroomsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetClassrooms, UserType.Manager, SykiPermissions.ManageClassrooms)
            .AddSykiPolicy(CreateClassroom, UserType.Manager, SykiPermissions.ManageClassrooms);

        return builder;
    }
}
