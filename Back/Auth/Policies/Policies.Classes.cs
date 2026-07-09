using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClasses = nameof(GetClasses);
    public const string CreateClass = nameof(CreateClass);

    public static AuthorizationBuilder AddClassesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetClasses, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(CreateClass, UserType.Manager, EstudPermissions.ManageClasses);

        return builder;
    }
}
