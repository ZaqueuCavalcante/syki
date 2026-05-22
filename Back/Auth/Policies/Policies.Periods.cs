using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetAcademicPeriods = nameof(GetAcademicPeriods);
    public const string CreateAcademicPeriod = nameof(CreateAcademicPeriod);

    public static AuthorizationBuilder AddAcademicPeriodsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetAcademicPeriods, SykiPermissions.ManagePeriods)
            .AddSykiPolicy(CreateAcademicPeriod, SykiPermissions.ManagePeriods);

        return builder;
    }
}
