using Syki.Back.Auth;

namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });

        services.AddSingleton<IAuthorizationHandler, CrossLoginAuthReqHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(BackPolicy.CrossLogin, p => p.Requirements.Add(new CrossLoginAuthRequirement()));
        });
    }
}
