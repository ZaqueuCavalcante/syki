using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string CreateCalendarDay = nameof(CreateCalendarDay);
    public const string UpdateCalendarDay = nameof(UpdateCalendarDay);
    public const string GetInstitutionCalendar = nameof(GetInstitutionCalendar);

    public static AuthorizationBuilder AddCalendarPolicies(this AuthorizationBuilder builder)
    {
        return builder
            .AddEstudPolicy(CreateCalendarDay, UserType.Manager, EstudPermissions.ManageCalendar)
            .AddEstudPolicy(UpdateCalendarDay, UserType.Manager, EstudPermissions.ManageCalendar)
            .AddEstudPolicy(GetInstitutionCalendar, UserType.Manager, EstudPermissions.ManageCalendar);
    }
}
