using Syki.Back.Auth.Permissions;

namespace Syki.Back.Auth.Policies;

public static partial class Policies
{
    public const string GetCourseCurriculums = nameof(GetCourseCurriculums);
    public const string CreateCourseCurriculum = nameof(CreateCourseCurriculum);

    public static AuthorizationBuilder AddCourseCurriculumsPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddSykiPolicy(GetCourseCurriculums, UserType.Manager, SykiPermissions.ManageCourseCurriculums)
            .AddSykiPolicy(CreateCourseCurriculum, UserType.Manager, SykiPermissions.ManageCourseCurriculums);

        return builder;
    }
}
