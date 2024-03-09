using Syki.Back.Login;
using Syki.Back.LoginMfa;
using Syki.Back.SetupMfa;
using Syki.Back.GetCampi;
using Syki.Back.GetMfaKey;
using Syki.Back.CreateUser;
using Syki.Back.GenerateJWT;
using Syki.Back.CreateCampus;
using Syki.Back.UpdateCampus;
using Syki.Back.CreateAluno;
using Syki.Back.ResetPassword;
using Syki.Back.LoginWithGoogle;
using Syki.Back.FinishUserRegister;
using Syki.Back.GetAcademicPeriods;
using Syki.Back.GetEnrollmentPeriods;
using Syki.Back.CreateAcademicPeriod;
using Syki.Back.SendResetPasswordToken;
using Syki.Back.CreatePendingUserRegister;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<FinishUserRegisterService>();
        services.AddScoped<ResetPasswordService>();
        services.AddScoped<SendResetPasswordEmailService>();

        services.AddScoped<CreateAcademicPeriodService>();
        services.AddScoped<GetAcademicPeriodsService>();
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<GetEnrollmentPeriodsService>();

        services.AddScoped<CreateAlunoService>();
        services.AddScoped<CreateUserService>();
        services.AddScoped<GenerateJWTService>();
        services.AddScoped<LoginService>();
        services.AddScoped<LoginWithGoogleService>();
        services.AddScoped<GetMfaKeyService>();
        services.AddScoped<SetupMfaService>();
        services.AddScoped<LoginMfaService>();
        services.AddScoped<CreateCampusService>();
        services.AddScoped<UpdateCampusService>();
        services.AddScoped<GetCampiService>();



        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailsService, EmailsService>();
        services.AddScoped<INotificationsService, NotificationsService>();

        if (Env.IsTesting() || Env.IsDevelopment())
        {
            services.Replace(ServiceDescriptor.Scoped<IEmailsService, FakeEmailsService>());
        }
    }
}
