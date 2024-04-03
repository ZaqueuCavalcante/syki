namespace Syki.Back;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigs();
        services.AddServicesConfigs();
        services.AddRateLimiterConfigs();

        services.AddIdentityConfigs();
        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddControllers();
        services.AddHttpConfigs();

        services.AddAuditConfigs();
        services.AddEfCoreConfigs();

        services.AddDocsConfigs();
        services.AddCorsConfigs();
    }

    public static void Configure(IApplicationBuilder app, SykiDbContext ctx)
    {
        ctx.ResetDb();

        app.UseCors();

        app.UseRouting();
        app.UseRateLimiter();
        app.UseDomainExceptions();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAudit();
        app.UseDocs();

        app.UseControllers();
    }
}
