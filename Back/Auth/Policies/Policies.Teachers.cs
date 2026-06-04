using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetTeacher = nameof(GetTeacher);
    public const string GetTeachers = nameof(GetTeachers);
    public const string CreateTeacher = nameof(CreateTeacher);
    public const string UpdateTeacher = nameof(UpdateTeacher);
    public const string GetTeacherPotentialCampi = nameof(GetTeacherPotentialCampi);
    public const string GetTeacherPotentialDisciplines = nameof(GetTeacherPotentialDisciplines);
    public const string AssignCampiToTeacher = nameof(AssignCampiToTeacher);
    public const string AssignDisciplinesToTeacher = nameof(AssignDisciplinesToTeacher);

    public static AuthorizationBuilder AddTeachersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetTeacher, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(GetTeachers, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(CreateTeacher, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(UpdateTeacher, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(GetTeacherPotentialCampi, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(GetTeacherPotentialDisciplines, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(AssignCampiToTeacher, UserType.Manager, SykiPermissions.ManageTeachers)
            .AddSykiPolicy(AssignDisciplinesToTeacher, UserType.Manager, SykiPermissions.ManageTeachers);

        return builder;
    }
}
