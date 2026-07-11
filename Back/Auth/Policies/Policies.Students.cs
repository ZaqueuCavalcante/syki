using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetStudent = nameof(GetStudent);
    public const string GetStudents = nameof(GetStudents);
    public const string CreateStudent = nameof(CreateStudent);
    public const string AssignStudentToClass = nameof(AssignStudentToClass);
    public const string EnrollStudentInCourseOffering = nameof(EnrollStudentInCourseOffering);

    public const string GetStudentAgenda = nameof(GetStudentAgenda);

    public static AuthorizationBuilder AddStudentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(GetStudents, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(CreateStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(AssignStudentToClass, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(EnrollStudentInCourseOffering, UserType.Manager, EstudPermissions.ManageStudents);

        builder
            .AddEstudPolicy(GetStudentAgenda, UserType.Student);

        return builder;
    }
}
