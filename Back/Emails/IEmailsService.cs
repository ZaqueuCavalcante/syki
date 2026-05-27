namespace Syki.Back.Emails;

public interface IEmailsService
{
    Task SendResetPasswordEmail(string to, string token);
    Task SendFirstAccessMagicLinkEmail(string to, string token);
}
