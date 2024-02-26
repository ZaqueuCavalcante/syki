using Syki.Back.Login;
using Syki.Back.GetBooks;
using Syki.Back.LoginMfa;
using Syki.Back.SetupMfa;
using Syki.Back.GetCampi;
using Syki.Back.GetMfaKey;
using Syki.Back.CreateBook;
using Syki.Back.Extensions;
using Syki.Back.CreateUser;
using Syki.Back.GenerateJWT;
using Syki.Back.CreateCampus;
using Syki.Back.UpdateCampus;
using Syki.Back.FinishUserRegister;
using Syki.Back.CreatePendingUserRegister;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<FinishUserRegisterService>();




        services.AddScoped<CreateBookService>();
        services.AddScoped<GetBooksService>();
        services.AddScoped<CreateUserService>();
        services.AddScoped<GenerateJWTService>();
        services.AddScoped<LoginService>();
        services.AddScoped<GetMfaKeyService>();
        services.AddScoped<SetupMfaService>();
        services.AddScoped<LoginMfaService>();
        services.AddScoped<CreateCampusService>();
        services.AddScoped<UpdateCampusService>();
        services.AddScoped<GetCampiService>();



        services.AddScoped<IAuthService, AuthService>();
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
