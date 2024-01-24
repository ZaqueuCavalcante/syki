namespace Syki.Back.Services;

public interface IEmailsService
{
    void Send(string email, string message);
}
