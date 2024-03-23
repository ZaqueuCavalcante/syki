namespace Syki.Daemon.Emails;

public interface IEmailsService
{
    Task SendResetPasswordEmail(string to, string token);
    Task SendUserRegisterEmailConfirmation(string to, string token);
}
