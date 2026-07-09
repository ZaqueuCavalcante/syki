using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClassrooms = nameof(GetClassrooms);
    public const string CreateClassroom = nameof(CreateClassroom);

    public static AuthorizationBuilder AddClassroomsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetClassrooms, UserType.Manager, EstudPermissions.ManageClassrooms)
            .AddEstudPolicy(CreateClassroom, UserType.Manager, EstudPermissions.ManageClassrooms);

        return builder;
    }
}
