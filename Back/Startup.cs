using Syki.Configs;
using Syki.Settings;

namespace Syki;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AuthSettings>();
        services.AddSingleton<DatabaseSettings>();

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
