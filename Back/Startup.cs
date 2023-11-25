using Syki.Back.Hubs;
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

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAlunosService, AlunosService>();
        services.AddScoped<ICampiService, CampiService>();
        services.AddScoped<ICursosService, CursosService>();
        services.AddScoped<IDisciplinasService, DisciplinasService>();
        services.AddScoped<IFaculdadesService, FaculdadesService>();
        services.AddScoped<IGradesService, GradesService>();
        services.AddScoped<IIndexService, IndexService>();
        services.AddScoped<ILivrosService, LivrosService>();
        services.AddScoped<IOfertasService, OfertasService>();
        services.AddScoped<IPeriodosService, PeriodosService>();
        services.AddScoped<IProfessoresService, ProfessoresService>();
        services.AddScoped<ITurmasService, TurmasService>();

        services.AddIdentityConfigs();

        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddSignalR();

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

        app.UseEndpoints(options =>
        {
            options.MapControllers();
            options.MapHub<NotificationsHub>("hubs/notifications");
        });
    }
}
