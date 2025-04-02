using Syki.Back.Auth;

namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });

        builder.Services.AddSingleton<IAuthorizationHandler, CrossLoginAuthReqHandler>();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(BackPolicy.CrossLogin, p => p.Requirements.Add(new CrossLoginAuthRequirement()));
    }
}
