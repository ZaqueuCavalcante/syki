using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourseOfferings = nameof(GetCourseOfferings);
    public const string CreateCourseOffering = nameof(CreateCourseOffering);

    public static AuthorizationBuilder AddCourseOfferingsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCourseOfferings, UserType.Manager, SykiPermissions.ManageCourseOfferings)
            .AddSykiPolicy(CreateCourseOffering, UserType.Manager, SykiPermissions.ManageCourseOfferings);

        return builder;
    }
}
