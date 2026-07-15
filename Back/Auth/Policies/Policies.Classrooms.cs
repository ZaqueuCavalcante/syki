using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClassroom = nameof(GetClassroom);
    public const string GetClassrooms = nameof(GetClassrooms);
    public const string CreateClassroom = nameof(CreateClassroom);
    public const string UpdateClassroom = nameof(UpdateClassroom);

    public static AuthorizationBuilder AddClassroomsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetClassroom, UserType.Manager, EstudPermissions.ManageClassrooms)
            .AddEstudPolicy(GetClassrooms, UserType.Manager, EstudPermissions.ManageClassrooms)
            .AddEstudPolicy(CreateClassroom, UserType.Manager, EstudPermissions.ManageClassrooms)
            .AddEstudPolicy(UpdateClassroom, UserType.Manager, EstudPermissions.ManageClassrooms);

        return builder;
    }
}
