using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetAcademicPeriods = nameof(GetAcademicPeriods);
    public const string CreateAcademicPeriod = nameof(CreateAcademicPeriod);

    public const string GetEnrollmentPeriods = nameof(GetEnrollmentPeriods);
    public const string CreateEnrollmentPeriod = nameof(CreateEnrollmentPeriod);

    public static AuthorizationBuilder AddAcademicPeriodsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetAcademicPeriods, UserType.Manager, EstudPermissions.ManagePeriods)
            .AddEstudPolicy(CreateAcademicPeriod, UserType.Manager, EstudPermissions.ManagePeriods);

        builder
            .AddEstudPolicy(GetEnrollmentPeriods, UserType.Manager, EstudPermissions.ManagePeriods)
            .AddEstudPolicy(CreateEnrollmentPeriod, UserType.Manager, EstudPermissions.ManagePeriods);

        return builder;
    }
}
