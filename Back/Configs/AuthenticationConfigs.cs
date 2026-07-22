using Estud.Back.Auth.Schemes;

namespace Estud.Back.Configs;

public static class AuthenticationConfigs
{
    public static void AddAuthenticationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options => options.DefaultChallengeScheme = JwtBearerScheme.Name)
            .AddJwtBearerScheme(builder.Configuration)
            .AddSsoTempCookieScheme()
            .AddTwoFactorSetupScheme()
            .AddSsoOpenIdConnectScheme()
            .AddSocialTempCookieScheme()
            .AddSocialLoginSchemes(builder.Configuration);
    }
}
