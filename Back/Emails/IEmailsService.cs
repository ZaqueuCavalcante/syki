namespace Syki.Back.Emails;

public interface IEmailsService
{
    Task SendFirstAccessMagicLinkEmail(string to, string token);







    Task SendResetPasswordEmail(string to, string token);
    Task SendUserRegisterEmailConfirmation(string to, string token);
    Task SendNewClassActivityEmail(string to, string message);
}
