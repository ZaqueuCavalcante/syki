using Syki.Back.Features.Cross.Login;
using Syki.Back.Features.Cross.SetupMfa;
using Syki.Back.Features.Cross.LoginMfa;
using Syki.Back.Features.Cross.GetMfaKey;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.GenerateJWT;
using Syki.Back.Features.Cross.ResetPassword;
using Syki.Back.Features.Cross.ViewNotifications;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.GetUserNotifications;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Configs;

public static class CrossServicesConfigs
{
    public static void AddCrossServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<CreateUserService>();
        services.AddScoped<FinishUserRegisterService>();
        services.AddScoped<GenerateJWTService>();
        services.AddScoped<GetMfaKeyService>();
        services.AddScoped<GetUserNotificationsService>();
        services.AddScoped<LoginService>();
        services.AddScoped<LoginMfaService>();
        services.AddScoped<ResetPasswordService>();
        services.AddScoped<SendResetPasswordTokenService>();
        services.AddScoped<SetupMfaService>();
        services.AddScoped<ViewNotificationsService>();
    }
}
