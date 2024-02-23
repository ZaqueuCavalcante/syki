using Syki.Back.Services;
using Syki.Back.GetBooks;
using Syki.Back.SetupDemo;
using Syki.Back.CreateBook;
using Syki.Back.Extensions;
using Syki.Back.CreateUser;
using Syki.Back.CreatePendingDemo;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<CreateBookService>();
        services.AddScoped<GetBooksService>();
        services.AddScoped<CreatePendingDemoService>();
        services.AddScoped<SetupDemoService>();
        services.AddScoped<CreateUserService>();


        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICampiService, CampiService>();
        services.AddScoped<IIndexService, IndexService>();
        services.AddScoped<ICursosService, CursosService>();
        services.AddScoped<IGradesService, GradesService>();
        services.AddScoped<IAlunosService, AlunosService>();
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
