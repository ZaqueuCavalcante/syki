namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddUsersPolicies()
            .AddCampiPolicies()
            .AddCoursesPolicies()
            .AddClassesPolicies()
            .AddIdentityPolicies()
            .AddTeachersPolicies()
            .AddStudentsPolicies()
            .AddDisciplinesPolicies()
            .AddNotificationsPolicies()
            .AddAcademicPeriodsPolicies()
            .AddCourseOfferingsPolicies()
            .AddCourseCurriculumsPolicies();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });
    }
}
