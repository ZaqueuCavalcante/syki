using Syki.Back.Services;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAlunosService, AlunosService>();
        services.AddScoped<ICampiService, CampiService>();
        services.AddScoped<ICursosService, CursosService>();
        services.AddScoped<IDisciplinasService, DisciplinasService>();
        services.AddScoped<IFaculdadesService, FaculdadesService>();
        services.AddScoped<IGradesService, GradesService>();
        services.AddScoped<IIndexService, IndexService>();
        services.AddScoped<ILivrosService, LivrosService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<IOfertasService, OfertasService>();
        services.AddScoped<IPeriodosService, PeriodosService>();
        services.AddScoped<IProfessoresService, ProfessoresService>();
        services.AddScoped<ITurmasService, TurmasService>();
        services.AddScoped<IEmailsService, EmailsService>();
        services.AddScoped<IAgendasService, AgendasService>();
    }
}
