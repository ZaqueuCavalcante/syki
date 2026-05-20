namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddUsersPolicies()
            .AddCampiPolicies()
            .AddCoursesPolicies()
            .AddIdentityPolicies()
            .AddDisciplinesPolicies();

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
