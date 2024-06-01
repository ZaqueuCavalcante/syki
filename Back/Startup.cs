namespace Syki.Back;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigs();

        services.AddAcademicServicesConfigs();
        services.AddCrossServicesConfigs();
        services.AddAdmServicesConfigs();
        services.AddTeacherServicesConfigs();
        services.AddStudentServicesConfigs();
        services.AddSellerServicesConfigs();

        services.AddRateLimiterConfigs();

        services.AddIdentityConfigs();
        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddHttpConfigs();

        services.AddAuditConfigs();
        services.AddDapperConfigs();
        services.AddEfCoreConfigs();

        services.AddDocsConfigs();
        services.AddCorsConfigs();
    }

    public static void Configure(IApplicationBuilder app, SykiDbContext ctx)
    {
        ctx.MigrateDb();

        app.UseSerilogRequestLogging();

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
