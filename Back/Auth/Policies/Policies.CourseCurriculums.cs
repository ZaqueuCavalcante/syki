using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourseCurriculum = nameof(GetCourseCurriculum);
    public const string GetCourseCurriculums = nameof(GetCourseCurriculums);
    public const string CreateCourseCurriculum = nameof(CreateCourseCurriculum);
    public const string EditCourseCurriculum = nameof(EditCourseCurriculum);

    public static AuthorizationBuilder AddCourseCurriculumsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCourseCurriculum, UserType.Manager, SykiPermissions.ManageCourseCurriculums)
            .AddSykiPolicy(GetCourseCurriculums, UserType.Manager, SykiPermissions.ManageCourseCurriculums)
            .AddSykiPolicy(CreateCourseCurriculum, UserType.Manager, SykiPermissions.ManageCourseCurriculums)
            .AddSykiPolicy(EditCourseCurriculum, UserType.Manager, SykiPermissions.ManageCourseCurriculums);

        return builder;
    }
}
