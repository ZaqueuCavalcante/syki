namespace Syki.Back.Services;

public interface IEmailsService
{
    Task SendResetPasswordEmail(string to, string token);
    Task SendDemoEmailConfirmation(string to, string token);
}
