namespace Syki.Back.Emails;

public class FakeEmailsService(EmailSettings settings) : IEmailsService
{
    public List<string> Emails = [];

    public async Task SendResetPasswordEmail(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{settings.FrontUrl}/reset-password?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        Emails.Add($"[{to} -> {link}]");
    }

    public async Task SendUserRegisterEmailConfirmation(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{settings.FrontUrl}/register-setup?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        Emails.Add($"[{to} -> {link}]");
    }
}
