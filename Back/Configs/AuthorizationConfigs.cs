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

        services.AddSingleton<IAuthorizationHandler, SkipUserRegisterAuthReqHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(BackPolicy.SkipUserRegister, p => p.Requirements.Add(new SkipUserRegisterAuthReq()));
        });
    }
}
