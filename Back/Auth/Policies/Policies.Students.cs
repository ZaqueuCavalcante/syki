using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetStudent = nameof(GetStudent);
    public const string GetStudents = nameof(GetStudents);
    public const string CreateStudent = nameof(CreateStudent);
    public const string GetStudentDetails = nameof(GetStudentDetails);
    public const string AssignStudentToClass = nameof(AssignStudentToClass);
    public const string EnrollStudentInCourseOffering = nameof(EnrollStudentInCourseOffering);

    public const string GetStudentClass = nameof(GetStudentClass);
    public const string GetStudentAgenda = nameof(GetStudentAgenda);
    public const string GetStudentCourseDetails = nameof(GetStudentCourseDetails);
    public const string GetStudentAttendanceCalendar = nameof(GetStudentAttendanceCalendar);

    public const string CreateClassActivityWork = nameof(CreateClassActivityWork);
    public const string GetStudentClassActivity = nameof(GetStudentClassActivity);
    public const string GetStudentClassActivities = nameof(GetStudentClassActivities);

    public static AuthorizationBuilder AddStudentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(GetStudents, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(CreateStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(GetStudentDetails, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(AssignStudentToClass, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(EnrollStudentInCourseOffering, UserType.Manager, EstudPermissions.ManageStudents);

        builder
            .AddEstudPolicy(GetStudentClass, UserType.Student)
            .AddEstudPolicy(GetStudentAgenda, UserType.Student)
            .AddEstudPolicy(GetStudentCourseDetails, UserType.Student)
            .AddEstudPolicy(GetStudentAttendanceCalendar, UserType.Student);

        builder
            .AddEstudPolicy(CreateClassActivityWork, UserType.Student)
            .AddEstudPolicy(GetStudentClassActivity, UserType.Student)
            .AddEstudPolicy(GetStudentClassActivities, UserType.Student);

        return builder;
    }
}
