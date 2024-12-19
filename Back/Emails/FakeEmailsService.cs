namespace Syki.Back.Emails;

public class FakeEmailsService(EmailSettings settings) : IEmailsService
{
    public List<string> ResetPasswordEmails = [];
    public List<string> UserRegisterEmailConfirmationEmails = [];

    public async Task SendResetPasswordEmail(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{settings.FrontUrl}/reset-password?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        ResetPasswordEmails.Add($"[{to} -> {link}]");
    }

    public async Task SendUserRegisterEmailConfirmation(string to, string token)
    {
        await Task.Delay(0);
        var link = $"{settings.FrontUrl}/register-setup?token={token}";
        Console.WriteLine($"[{to} -> {link}]");
        UserRegisterEmailConfirmationEmails.Add($"[{to} -> {link}]");
    }
}
