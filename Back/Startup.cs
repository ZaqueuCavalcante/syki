using Syki.Back.Hubs;
using Syki.Back.Configs;
using Syki.Back.Exceptions;

namespace Syki.Back;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigs();
        services.AddServicesConfigs();

        services.AddIdentityConfigs();
        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddSignalR();
        services.AddControllers();
        services.AddHttpConfigs();

        services.AddAuditConfigs();
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

        app.UseAudit();

        app.UseMiddleware<DomainExceptionMiddleware>();

        app.UseEndpoints(options =>
        {
            options.MapControllers();
            options.MapHub<NotificationsHub>("hubs/notifications");
        });
    }
}
