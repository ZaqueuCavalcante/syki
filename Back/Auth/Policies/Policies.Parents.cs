using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetParents = nameof(GetParents);
    public const string CreateParent = nameof(CreateParent);
    public const string GetParentDetails = nameof(GetParentDetails);

    public const string GetParentStudents = nameof(GetParentStudents);
    public const string GetParentStudentAgenda = nameof(GetParentStudentAgenda);

    public static AuthorizationBuilder AddParentsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetParents, UserType.Manager, EstudPermissions.ManageParents)
            .AddEstudPolicy(CreateParent, UserType.Manager, EstudPermissions.ManageParents)
            .AddEstudPolicy(GetParentDetails, UserType.Manager, EstudPermissions.ManageParents);

        builder
            .AddEstudPolicy(GetParentStudents, UserType.Parent)
            .AddEstudPolicy(GetParentStudentAgenda, UserType.Parent);

        return builder;
    }
}
