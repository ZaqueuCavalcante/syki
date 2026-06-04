using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClasses = nameof(GetClasses);
    public const string CreateClass = nameof(CreateClass);

    public static AuthorizationBuilder AddClassesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetClasses, UserType.Manager, SykiPermissions.ManageClasses)
            .AddSykiPolicy(CreateClass, UserType.Manager, SykiPermissions.ManageClasses);

        return builder;
    }
}
