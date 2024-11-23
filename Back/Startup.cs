namespace Syki.Back;

public class Startup()
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigs();
        services.AddServicesConfigs();

        services.AddIdentityConfigs();
        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddHttpConfigs();
        services.AddRateLimiterConfigs();

        services.AddAuditConfigs();
        services.AddDapperConfigs();
        services.AddEfCoreConfigs();

        services.AddDocsConfigs();
        services.AddCorsConfigs();

        services.AddOpenApi();
    }

    public static void Configure(IApplicationBuilder app, SykiDbContext ctx)
    {
        ctx.ResetDb();

        app.UseLogs();

        app.UseCors();

        app.UseRouting();
        app.UseRateLimiter();
        app.UseExceptions();
        app.UseCustomHeaders();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAudit();
        app.UseDocs();

        app.UseControllers();
    }
}
