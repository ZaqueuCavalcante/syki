using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetClass = nameof(GetClass);
    public const string StartClass = nameof(StartClass);
    public const string GetClasses = nameof(GetClasses);
    public const string CreateClass = nameof(CreateClass);
    public const string UpdateClassTeachers = nameof(UpdateClassTeachers);
    public const string UpdateClassSchedules = nameof(UpdateClassSchedules);
    public const string ReleaseClassForEnrollment = nameof(ReleaseClassForEnrollment);

    public static AuthorizationBuilder AddClassesPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetClass, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(StartClass, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(GetClasses, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(CreateClass, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(UpdateClassTeachers, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(UpdateClassSchedules, UserType.Manager, EstudPermissions.ManageClasses)
            .AddEstudPolicy(ReleaseClassForEnrollment, UserType.Manager, EstudPermissions.ManageClasses);

        return builder;
    }
}
