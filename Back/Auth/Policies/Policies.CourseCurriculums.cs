using Estud.Back.Auth.Permissions;

namespace Estud.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourseCurriculum = nameof(GetCourseCurriculum);
    public const string GetCourseCurriculums = nameof(GetCourseCurriculums);
    public const string CreateCourseCurriculum = nameof(CreateCourseCurriculum);
    public const string UpdateCourseCurriculum = nameof(UpdateCourseCurriculum);

    public static AuthorizationBuilder AddCourseCurriculumsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddEstudPolicy(GetCourseCurriculum, UserType.Manager, EstudPermissions.ManageCourseCurriculums)
            .AddEstudPolicy(GetCourseCurriculums, UserType.Manager, EstudPermissions.ManageCourseCurriculums)
            .AddEstudPolicy(CreateCourseCurriculum, UserType.Manager, EstudPermissions.ManageCourseCurriculums)
            .AddEstudPolicy(UpdateCourseCurriculum, UserType.Manager, EstudPermissions.ManageCourseCurriculums);

        return builder;
    }
}
