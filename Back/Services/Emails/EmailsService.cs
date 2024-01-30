namespace Syki.Back.Services;

public class EmailsService : IEmailsService
{
    public void Send(string email, string message)
    {
        Console.WriteLine($"Email sended to [{email}] with message [{message}]");
    }
}
