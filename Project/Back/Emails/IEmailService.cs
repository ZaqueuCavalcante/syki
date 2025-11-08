namespace Exato.Back.Emails;

public interface IEmailService
{
    Task SendResetPasswordEmail(string to, string token);
}
