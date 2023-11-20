using Syki.Back.Configs;
using Syki.Back.Services;
using Syki.Back.Settings;

namespace Syki.Back;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AuthSettings>();
        services.AddSingleton<DatabaseSettings>();

        services.AddScoped<AuthService>();
        services.AddScoped<IAlunosService, AlunosService>();
        services.AddScoped<ICampiService, CampiService>();

        services.AddIdentityConfigs();
        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddControllers();

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddEfCoreConfigs();

        services.AddSwaggerConfigs();

        services.AddCorsConfigs();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseCors();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwaggerThings();

        app.UseEndpoints(options => options.MapControllers());
    }
}
