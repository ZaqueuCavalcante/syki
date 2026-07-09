using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetAcademicPeriods = nameof(GetAcademicPeriods);
    public const string CreateAcademicPeriod = nameof(CreateAcademicPeriod);

    public static AuthorizationBuilder AddAcademicPeriodsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetAcademicPeriods, UserType.Manager, EstudPermissions.ManagePeriods)
            .AddEstudPolicy(CreateAcademicPeriod, UserType.Manager, EstudPermissions.ManagePeriods);

        return builder;
    }
}
