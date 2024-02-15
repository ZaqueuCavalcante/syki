using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICampiService, CampiService>();
        services.AddScoped<IDemosService, DemosService>();
        services.AddScoped<IIndexService, IndexService>();
        services.AddScoped<ICursosService, CursosService>();
        services.AddScoped<IGradesService, GradesService>();
        services.AddScoped<IAlunosService, AlunosService>();
        services.AddScoped<ILivrosService, LivrosService>();
        services.AddScoped<IEmailsService, EmailsService>();
        services.AddScoped<ITurmasService, TurmasService>();
        services.AddScoped<IOfertasService, OfertasService>();
        services.AddScoped<IAgendasService, AgendasService>();
        services.AddScoped<IPeriodosService, PeriodosService>();
        services.AddScoped<IFaculdadesService, FaculdadesService>();
        services.AddScoped<IMatriculasService, MatriculasService>();
        services.AddScoped<IDisciplinasService, DisciplinasService>();
        services.AddScoped<IProfessoresService, ProfessoresService>();
        services.AddScoped<INotificationsService, NotificationsService>();

        if (Env.IsTesting() || Env.IsDevelopment())
        {
            services.Replace(ServiceDescriptor.Scoped<IEmailsService, FakeEmailsService>());
        }
    }
}
