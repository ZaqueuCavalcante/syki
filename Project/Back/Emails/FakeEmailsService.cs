namespace Exato.Back.Emails;

public class FakeEmailService(IConfiguration configuration) : IEmailService
{
    public List<string> ResetPasswordEmails = [];

    public async Task SendResetPasswordEmail(string to, string token)
    {
        var settings = configuration.Email;
        await Task.Yield();
        var link = $"{settings.FrontUrl}/reset-password?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        ResetPasswordEmails.Add($"[{to} -> {link}]");
    }
}
