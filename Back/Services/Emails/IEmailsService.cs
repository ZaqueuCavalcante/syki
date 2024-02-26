namespace Syki.Back.Services;

public interface IEmailsService
{
    Task SendResetPasswordEmail(string to, string token);
    Task SendUserRegisterEmailConfirmation(string to, string token);
}
