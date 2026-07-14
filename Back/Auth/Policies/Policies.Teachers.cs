using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

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

    public const string GetTeacherClass = nameof(GetTeacherClass);
    public const string GetTeacherAgenda = nameof(GetTeacherAgenda);
    public const string GetTeacherCurrentClasses = nameof(GetTeacherCurrentClasses);

    public const string CreateClassActivity = nameof(CreateClassActivity);
    public const string GetTeacherClassActivity = nameof(GetTeacherClassActivity);
    public const string GetTeacherClassActivities = nameof(GetTeacherClassActivities);

    public static AuthorizationBuilder AddTeachersPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetTeacher, UserType.Manager, EstudPermissions.ManageTeachers)
            .AddEstudPolicy(GetTeachers, UserType.Manager, EstudPermissions.ManageTeachers)
            .AddEstudPolicy(CreateTeacher, UserType.Manager, EstudPermissions.ManageTeachers)
            .AddEstudPolicy(UpdateTeacher, UserType.Manager, EstudPermissions.ManageTeachers);

        builder
            .AddEstudPolicy(GetTeacherPotentialCampi, UserType.Manager, EstudPermissions.ManageTeachers)
            .AddEstudPolicy(GetTeacherPotentialDisciplines, UserType.Manager, EstudPermissions.ManageTeachers);

        builder
            .AddEstudPolicy(AssignCampiToTeacher, UserType.Manager, EstudPermissions.ManageTeachers)
            .AddEstudPolicy(AssignDisciplinesToTeacher, UserType.Manager, EstudPermissions.ManageTeachers);

        builder
            .AddEstudPolicy(GetTeacherClass, UserType.Teacher)
            .AddEstudPolicy(GetTeacherAgenda, UserType.Teacher)
            .AddEstudPolicy(GetTeacherCurrentClasses, UserType.Teacher);

        builder
            .AddEstudPolicy(CreateClassActivity, UserType.Teacher)
            .AddEstudPolicy(GetTeacherClassActivity, UserType.Teacher)
            .AddEstudPolicy(GetTeacherClassActivities, UserType.Teacher);

        return builder;
    }
}
