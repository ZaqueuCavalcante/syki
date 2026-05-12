using Syki.Back.Auth.Schemes;

namespace Syki.Back.Configs;

public static class AuthenticationConfigs
{
    public static void AddAuthenticationConfigs(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options => options.DefaultChallengeScheme = JwtBearerScheme.Name)
            .AddJwtBearerScheme(builder.Configuration);
    }
}
