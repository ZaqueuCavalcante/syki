using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetStudents = nameof(GetStudents);
    public const string GetStudent = nameof(GetStudent);
    public const string CreateStudent = nameof(CreateStudent);
    public const string EnrollStudentInCourseOffering = nameof(EnrollStudentInCourseOffering);

    public static AuthorizationBuilder AddStudentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetStudents, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(GetStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(CreateStudent, UserType.Manager, EstudPermissions.ManageStudents)
            .AddEstudPolicy(EnrollStudentInCourseOffering, UserType.Manager, EstudPermissions.ManageStudents);

        return builder;
    }
}
