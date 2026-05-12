namespace Syki.Back.Emails;

public interface IEmailsService
{
    Task SendFirstAccessMagicLinkEmail(string to, string token);
}
