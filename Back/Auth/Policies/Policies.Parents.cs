using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateParent = nameof(CreateParent);

    public const string GetParentStudents = nameof(GetParentStudents);

    public static AuthorizationBuilder AddParentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(CreateParent, UserType.Manager, EstudPermissions.ManageParents);

        builder
            .AddEstudPolicy(GetParentStudents, UserType.Parent);

        return builder;
    }
}
